using Library8.Models;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
namespace TaskManagementAPI.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto);

        Task<TaskManagementAPI.Models.User?> GetUserByEmailAsync(string email);
        Task<Models.User> RegisterUserAsync(string fullName, string email, string password, string role);
    }

}
