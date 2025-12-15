using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementAPI.Services.Interfaces;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services
{
 

    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;

        public JwtService(IConfiguration config)
        {
            _config = config;
        }
        private string GetRoleName(int roleId)
        {
            return roleId switch
            {
                1 => "Admin",
                2 => "ProjectManager",
                3 => "Employee",
                _ => "Employee"
            };
        }

        public string GenerateToken(User user)
        {
            var roleName = GetRoleName(user.RoleId);
            var claims = new List<Claim>
        {
            new Claim("UserId", user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
               new Claim("role", roleName)

        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_config["Jwt:Key"])
            );

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

}
