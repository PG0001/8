using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginAsync(LoginDto dto);
    Task<AuthResponseDto> RegisterAsync(string fullName, string email, string password, string roleName);
    Task<User?> GetUserByEmailAsync(string email);
    Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword);
}
