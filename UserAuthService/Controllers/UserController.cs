using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using User_Service.Models;
using User_Service.Service;

namespace User_Service.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _userService;

        public UsersController(UserService userService)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] UserModel newUser)
        {
            try
            {
                var registeredUser = _userService.RegisterUser(newUser);
                return Ok(registeredUser);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequestModel loginRequest)
        {
            try
            {
                var authenticatedUser = _userService.AuthenticateUser(loginRequest.Email, loginRequest.PasswordHash);
                return Ok(authenticatedUser);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("get-all-users")]
        [Authorize(Roles ="Admin")] 
        public IActionResult GetAllUsers()
        {
            try
            {
                var allUsers = _userService.GetAllUsers(GetUserIdFromToken());
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

            [HttpPost("change-user-role/{userId}/{newRole}")]
        [Authorize(Roles = "Admin")] 
        public IActionResult ChangeUserRole(int userId, string newRole)
        {
            try
            {
                _userService.ChangeUserRole(GetUserIdFromToken(), userId, newRole);
                return Ok($"Role for user with ID {userId} changed to '{newRole}' successfully.");
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private int GetUserIdFromToken()
        {
            var userIdClaim = User?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            return userIdClaim != null ? Convert.ToInt32(userIdClaim.Value) : 0;
        }
    }

}
