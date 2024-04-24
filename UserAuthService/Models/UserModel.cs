namespace User_Service.Models
{
    // UserModel.cs
    public class UserModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; } 
        public string MobileNumber { get; set; }
        public string FullName { get; set; }
    }

    public class AuthenticatedUserModel : UserModel
    {
        public string Token { get; set; }
    }

}
