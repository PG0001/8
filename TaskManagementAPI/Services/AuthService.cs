using Library8.Models.Interfaces;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Services.Interfaces;
using BCrypt.Net;

namespace TaskManagementAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IJwtService _jwtService;

        public AuthService(IUserRepository userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }

        // ---------------- LOGIN ----------------
        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmailAsync(dto.Email);
            if (user == null) return null;

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return null;
            var user1 = new TaskManagementAPI.Models.User
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                RoleId = user.RoleId
            };
            var token = _jwtService.GenerateToken(user1);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = user.Role.Name
            };
        }

        // ---------------- REGISTER ----------------
        public async Task<AuthResponseDto> RegisterAsync(string fullName, string email, string password, string roleName)
        {
            var existingUser = await _userRepo.GetByEmailAsync(email);
            if (existingUser != null)
                throw new Exception("User with this email already exists.");

            // Check if role exists
            var role = await _userRepo.GetRoleByNameAsync(roleName);
            if (role == null)
            {
                role = await _userRepo.AddRoleAsync(new Library8.Models.Role { Name = roleName });
            }

            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            var user = new Library8.Models.User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                RoleId = role.Id
            };

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

          
            var user1= new TaskManagementAPI.Models.User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = hash,
                RoleId = role.Id
            };

            var token = _jwtService.GenerateToken(user1);

            return new AuthResponseDto
            {
                Token = token,
                Email = user.Email,
                Role = role.Name
            };
        }

        // ---------------- HELPER ----------------
        public async Task<TaskManagementAPI.Models.User?> GetUserByEmailAsync(string email)
        {
            var neww= await _userRepo.GetByEmailAsync(email);

            if (neww == null) return null;
            return new TaskManagementAPI.Models.User
            {
                Id = neww.Id,
                FullName = neww.FullName,
                Email = neww.Email,
                PasswordHash = neww.PasswordHash,
                RoleId = neww.RoleId,
                CreatedOn = neww.CreatedOn
            };
        }
        public async Task<bool> ChangePasswordAsync(string email, string currentPassword, string newPassword)
        {
            var user = await _userRepo.GetByEmailAsync(email);
            if (user == null) return false;
            if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.PasswordHash))
                return false;
            var newHash = BCrypt.Net.BCrypt.HashPassword(newPassword);
            user.PasswordHash = newHash;
            _userRepo.Update(user);
            await _userRepo.SaveChangesAsync();
            return true;
        }
    }
}
