using Library8;
using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
public class UserRepository : Repository<User>, IUserRepository
{

    public readonly DBContext _context;
    public UserRepository(DBContext context) : base(context) {
    
        _context = context;
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Role)
            .FirstOrDefaultAsync(u => u.Email == email);
    }
    public async Task<List<Role>> GetRolesAsync()
    {
        return await _context.Roles.ToListAsync();
    }
    public async Task<Role?> GetRoleByNameAsync(string roleName)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == roleName);
    }

    public async Task<Role> AddRoleAsync(Role role)
    {
       _context.Roles.Add(role);
        return  await _context.Roles
            .FirstOrDefaultAsync(r => r.Name == role.Name);
    }
}

