using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Library8
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly DBContext _context;

        public ProjectRepository(DBContext context)
        {
            _context = context;
        }

        // ---------------- ADD PROJECT ----------------
        public async Task AddAsync(Project project)
        {
            await _context.Projects.AddAsync(project);
        }
        public async Task<List<ProjectUser>> GetAssignedUsersAsync(int projectId)
        {
            return await _context.ProjectUsers
                .Where(pu => pu.ProjectId == projectId)
                .Include(pu => pu.User)
                    .ThenInclude(u => u.Role)
                .ToListAsync();
        }


        // ---------------- SAVE CHANGES ----------------
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ---------------- GET ALL PROJECTS ----------------
        public async Task<List<Project>> GetAllAsync()
        {
            return await _context.Projects
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .ToListAsync();
        }

        // ---------------- GET PROJECT BY ID ----------------
        public async Task<Project?> GetByIdAsync(int projectId)
        {
            return await _context.Projects
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .FirstOrDefaultAsync(p => p.Id == projectId);
        }

        // ---------------- GET PROJECTS ASSIGNED TO USER ----------------
        public async Task<List<Project>> GetProjectsByUserAsync(int userId)
        {
            return await _context.Projects
                .Where(p => p.ProjectUsers.Any(pu => pu.UserId == userId))
                .Include(p => p.ProjectUsers)
                    .ThenInclude(pu => pu.User)
                .ToListAsync();
        }

        // ---------------- ADD USER TO PROJECT ----------------
        public async Task<bool> AddUserToProjectAsync(int projectId, int userId)
        {
            var project = await _context.Projects
                .Include(p => p.ProjectUsers)
                .FirstOrDefaultAsync(p => p.Id == projectId);

            if (project == null) return false;

            if (project.ProjectUsers.Any(pu => pu.UserId == userId))
                return false;

            var user = await _context.Users.FindAsync(userId);
            if (user == null) return false;

            var projectUser = new ProjectUser
            {
                ProjectId = projectId,
                UserId = userId,
                AddedOn = DateTime.UtcNow
            };

            project.ProjectUsers.Add(projectUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // ---------------- REMOVE USER FROM PROJECT ----------------
        public async Task<bool> RemoveUserFromProjectAsync(int projectId, int userId)
        {
            var projectUser = await _context.ProjectUsers
                .FirstOrDefaultAsync(pu => pu.ProjectId == projectId && pu.UserId == userId);

            if (projectUser == null) return false;

            _context.ProjectUsers.Remove(projectUser);
            await _context.SaveChangesAsync();
            return true;
        }

        // ---------------- DELETE PROJECT ----------------
        public void Remove(Project project)
        {
            _context.Projects.Remove(project);
        }
        public async Task<IEnumerable<Project>> SearchAsync(string? keyword)
        {
            var query = _context.Projects.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(p =>
                    p.Name.Contains(keyword) ||
                    p.Description!.Contains(keyword));
            }

            return await query.ToListAsync();
        }

    }
}
