using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;

namespace TaskManagementAPI.Services.Interfaces
{
    public interface IProjectService
    {   // Create a new project
        Task<Project> CreateProjectAsync(ProjectCreateDto dto, int userId);

        // Get all projects
        Task<List<Project>> GetAllAsync();

        // Get projects assigned to a specific user
        Task<List<Project>> GetProjectsByUserAsync(int userId);
        Task<List<UserDto>> GetAssignedUsersAsync(int projectId);

        // Get project by Id
        Task<Project?> GetByIdAsync(int projectId);

        // Update project details
        Task<Project?> UpdateProjectAsync(int projectId, ProjectUpdateDto dto);

        // Delete a project
        Task<bool> DeleteProjectAsync(int projectId);

        // Assign a user to a project
        Task<bool> AddUserToProjectAsync(int projectId, int userId);

        // Remove a user from a project
        Task<bool> RemoveUserFromProjectAsync(int projectId, int userId);

        Task<IEnumerable<ProjectDto>> SearchAsync(string? keyword);
    }

}
