using BCrypt.Net;
using Library8.Models;
using Library8.Models.Interfaces;
using System.Linq;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Interfaces;
public class UserService : IUserService
{
    private readonly IUserRepository _repo;
    public UserService(IUserRepository repo) => _repo = repo;

    public async Task<TaskManagementAPI.Models.User?> GetUserByEmailAsync(string email)
    {
        var user = await _repo.GetByEmailAsync(email);
        if (user == null) return null;

        var safe =ToSafeUser(user);
        return safe;
    }
    private static TaskManagementAPI.Models.User ToSafeUser(Library8.Models.User user)
    {
        return new TaskManagementAPI.Models.User
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            RoleId = user.RoleId
        };
    }

    public async Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto)
    {
        var user = await _repo.GetByIdAsync(userId);
        if (user == null) return false;

        user.FullName = dto.FullName;
        await _repo.SaveChangesAsync();
        return true;
    }

    public async Task<TaskManagementAPI.Models.User> RegisterUserAsync(
      string fullName,
      string email,
      string password,
      string roleName)
    {
        // 1. Check if user exists
        if (await _repo.GetByEmailAsync(email) != null)
            throw new Exception("User with this email already exists.");

        // 2. Get role
        var role = await _repo.GetRoleByNameAsync(roleName);

        // 3. Create role if not exists
        if (role == null)
        {
            role = await _repo.AddRoleAsync(new Library8.Models.Role
            {
                Name = roleName
            });
        }

        // 4. Hash password
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        // 5. Create user (IMPORTANT PART)
        var user = new Library8.Models.User
        {
            FullName = fullName,
            Email = email,
            PasswordHash = hash,
            RoleId = role.Id   // ✅ FK only
        };

        await _repo.AddAsync(user);
        await _repo.SaveChangesAsync();
        var user1 = new TaskManagementAPI.Models.User
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            RoleId = user.RoleId,
            CreatedOn = user.CreatedOn
        };
        return user1; // (Controller must map to DTO)
    }

}
