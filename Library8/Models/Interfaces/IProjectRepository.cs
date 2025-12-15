using Library8.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface IProjectRepository
    {
        // Add a new project
        Task AddAsync(Project project);

        // Save changes to the database
        Task SaveChangesAsync();

        // Get all projects
        Task<List<Project>> GetAllAsync();
        Task<List<ProjectUser>> GetAssignedUsersAsync(int projectId);

        // Get project by Id
        Task<Project?> GetByIdAsync(int projectId);

        // Get all projects assigned to a specific user
        Task<List<Project>> GetProjectsByUserAsync(int userId);

        // Assign a user to a project
        Task<bool> AddUserToProjectAsync(int projectId, int userId);

        // Remove a user from a project
        Task<bool> RemoveUserFromProjectAsync(int projectId, int userId);

        // Delete a project
        void Remove(Project project);
        Task<IEnumerable<Project>> SearchAsync(string? keyword);
    }
}
