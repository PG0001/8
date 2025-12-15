using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services.Interfaces
{

    public interface IJwtService
    {
        string GenerateToken(User user);
    }

}
