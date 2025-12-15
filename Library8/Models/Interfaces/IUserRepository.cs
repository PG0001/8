using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<List<Role>> GetRolesAsync();
        Task<Role?> GetRoleByNameAsync(string roleName);
        Task<Role> AddRoleAsync(Role role);
    }


}
