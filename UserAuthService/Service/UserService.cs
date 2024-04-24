using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User_Service.Models;

namespace User_Service.Service
{
    public class UserService
    {
        private readonly List<UserModel> _users;
        private readonly List<UserRoleModel> _userRoles;
        private readonly List<RoleModel> _roles;
        private readonly IConfiguration _configuration;

        public UserService(List<UserModel> users, List<UserRoleModel> userRoles, List<RoleModel> roles, IConfiguration configuration)
        {
            Console.WriteLine("constructor start");
            _users = users;
            _userRoles = userRoles;
            _roles = roles;
            _configuration = configuration;
            EnsureRoleExists("Buyer");
            EnsureRoleExists("Admin");
            EnsureAdminUserExists();
            _configuration = configuration;
        }

        public AuthenticatedUserModel RegisterUser(UserModel newUser)
        {
            if (_users.Any(u => u.Email == newUser.Email))
            {
                throw new ArgumentException("Email is already registered.");
            }

            newUser.Id = _users.Count + 1;
            newUser.PasswordHash = HashPassword(newUser.PasswordHash);

            _users.Add(newUser);

            var role = _roles.FirstOrDefault(r => r.Role == "Buyer");
            if (role != null)
            {
                _userRoles.Add(new UserRoleModel { UserId = newUser.Id, RoleId = role.Id });
            }

            return GenerateToken(newUser); 
        }

        public AuthenticatedUserModel AuthenticateUser(string email, string password)
        {
            var user = _users.FirstOrDefault(u => u.Email == email);

            if (user == null || !VerifyPassword(password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password.");
            }

            return GenerateToken(user); 
        }

        public IEnumerable<UserModel> GetAllUsers(int adminUserId)
        {
            if (!IsUserAdmin(adminUserId))
            {
                throw new UnauthorizedAccessException("Only admin can access this resource.");
            }

            return _users;
        }

        public void ChangeUserRole(int adminUserId, int userId, string newRole)
        {
            if (!IsUserAdmin(adminUserId))
            {
                throw new UnauthorizedAccessException("Only admin can change user roles.");
            }

            var user = _users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                throw new ArgumentException("User not found.");
            }

            var role = _roles.FirstOrDefault(r => r.Role == newRole);
            if (role == null)
            {
                throw new ArgumentException("Role not found.");
            }

            if (_userRoles.Any(ur => ur.UserId == userId && ur.RoleId == role.Id))
            {
                throw new InvalidOperationException($"User already has the '{newRole}' role.");
            }

            _userRoles.RemoveAll(ur => ur.UserId == userId);
            _userRoles.Add(new UserRoleModel { UserId = userId, RoleId = role.Id });
        }

        private AuthenticatedUserModel GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,  GetUserRoles(user.Id)),
             };

            var tokenDescriptor = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(15), 
                signingCredentials: credentials
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenString = tokenHandler.WriteToken(tokenDescriptor);


            var authenticatedUser = new AuthenticatedUserModel
            {
                Id = user.Id,
                Email = user.Email,
                MobileNumber = user.MobileNumber,
                FullName = user.FullName,
                Token = tokenString
            };
            Console.WriteLine($"User Roles: {GetUserRoles(user.Id)}");

            return authenticatedUser;
        }

        private string GetUserRoles(int userId)
        {
            var roles = _userRoles.Where(ur => ur.UserId == userId)
                                 .Join(_roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.Role);

            return string.Join(",", roles);
        }

        private bool IsUserAdmin(int userId)
        {
            return _userRoles.Any(ur => ur.UserId == userId && _roles.Any(r => r.Id == ur.RoleId && r.Role == "Admin"));
        }

        private string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }

        private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPasswordHash);
        }

        private void EnsureRoleExists(string roleName)
        {
            if (!_roles.Any(r => r.Role == roleName))
            {
                _roles.Add(new RoleModel { Id = _roles.Count + 1, Role = roleName });
            }
        }

        private void EnsureAdminUserExists()
        {
            var adminRole = _roles.First(r => r.Role == "Admin");

            if (!_users.Any(u => _userRoles.Any(ur => ur.UserId == u.Id && ur.RoleId == adminRole.Id)))
            {
                var adminUser = new UserModel
                {
                    Email = "admin@example.com",
                    PasswordHash = HashPassword("admin123"), 
                    MobileNumber = "1234567890",
                    FullName = "Admin User"
                };

                adminUser.Id = _users.Count + 1;
                _users.Add(adminUser);

                _userRoles.Add(new UserRoleModel { UserId = adminUser.Id, RoleId = adminRole.Id });
            }
        }
    }

}
