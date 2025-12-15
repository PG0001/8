namespace TaskManagementAPI.Dtos
{
    // Dtos/LoginDto.cs
    public class LoginDto
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    // Dtos/RegisterDto.cs
    public class RegisterDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Role { get; set; } = "Employee"; // default role
    }

    // Dtos/AuthResponseDto.cs
    public class AuthResponseDto
    {
        public string Token { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; set; } = null!;
    }
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }

}
