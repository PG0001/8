using Library8;
using Library8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _repo;

        public ProjectService(IProjectRepository repo)
        {
            _repo = repo;
        }
        public async Task<IEnumerable<ProjectDto>> SearchAsync(string? keyword)
        {
            var projects = await _repo.SearchAsync(keyword);

            return projects.Select(p => new ProjectDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Status = p.Status,
                StartDate = p.StartDate,
                EndDate = p.EndDate
            });
        }

        public async Task<List<UserDto>> GetAssignedUsersAsync(int projectId)
        {
            var users = await _repo.GetAssignedUsersAsync(projectId);
            

            return users.Select(u => new UserDto
            {
                Id = u.UserId,
                FullName = u.User.FullName,
                Email = u.User.Email,
                Role = u.User.Role.Name
            }).ToList();
        }

        public async Task<Project> CreateProjectAsync(ProjectCreateDto dto, int userId)
        {
            var project = new Library8.Models.Project
            {
                Name = dto.Name,
                Description = dto.Description,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                CreatedBy = userId,
                Status = "Active"
            };

            await _repo.AddAsync(project);
            await _repo.SaveChangesAsync();

            return MapToProjectDto(project);
        }

        public async Task<List<Project>> GetAllAsync()
        {
            var projects = await _repo.GetAllAsync();
            return projects.Select(MapToProjectDto).ToList();
        }

        public async Task<List<Project>> GetProjectsByUserAsync(int userId)
        {
            var projects = await _repo.GetProjectsByUserAsync(userId);
            return projects.Select(MapToProjectDto).ToList();
        }

        public async Task<Project?> GetByIdAsync(int projectId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return null;
            return MapToProjectDto(project);
        }

        public async Task<Project?> UpdateProjectAsync(int projectId, ProjectUpdateDto dto)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return null;

            project.Name = dto.Name ?? project.Name;
            project.Description = dto.Description ?? project.Description;
            project.StartDate = dto.StartDate ?? project.StartDate;
            project.EndDate = dto.EndDate ?? project.EndDate;
            project.Status = dto.Status ?? project.Status;

            await _repo.SaveChangesAsync();
            return MapToProjectDto(project);
        }

        public async Task<bool> DeleteProjectAsync(int projectId)
        {
            var project = await _repo.GetByIdAsync(projectId);
            if (project == null) return false;

            _repo.Remove(project);
            await _repo.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AddUserToProjectAsync(int projectId, int userId)
        {
            return await _repo.AddUserToProjectAsync(projectId, userId);
        }

        public async Task<bool> RemoveUserFromProjectAsync(int projectId, int userId)
        {
            return await _repo.RemoveUserFromProjectAsync(projectId, userId);
        }

        // Helper method to map Library8.Models.Project -> TaskManagementAPI.Models.Project DTO
        private Project MapToProjectDto(Library8.Models.Project project)
        {
            return new Project
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                StartDate = project.StartDate,
                EndDate = project.EndDate,
                CreatedBy = project.CreatedBy,
                Status = project.Status
            };
        }
    }
}
