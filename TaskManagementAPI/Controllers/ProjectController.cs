using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/projects")]
    [Authorize]
    public class ProjectController : ControllerBase
    {
        private readonly IProjectService _projectService;

        public ProjectController(IProjectService projectService)
        {
            _projectService = projectService;
        }
        [HttpGet("{projectId}/members")]
        public async Task<IActionResult> GetAssignedUsers(int projectId)
        {
            var users = await _projectService.GetAssignedUsersAsync(projectId);
            return Ok(users);
        }

        // ---------------- CREATE PROJECT (Admin/ProjectManager) ----------------
        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var project = await _projectService.CreateProjectAsync(dto, userId);
            return Ok(project);
        }

        // ---------------- GET ALL PROJECTS ----------------
        [HttpGet]
        public async Task<IActionResult> GetAllProjects()
        {
            var projects = await _projectService.GetAllAsync();
            return Ok(projects);
        }

        // ---------------- GET PROJECT BY ID ----------------
        [HttpGet("{projectId}")]
        public async Task<IActionResult> GetProjectById(int projectId)
        {
            var project = await _projectService.GetByIdAsync(projectId);
            if (project == null) return NotFound();
            return Ok(project);
        }

        // ---------------- GET PROJECTS ASSIGNED TO CURRENT USER ----------------
        [HttpGet("my-projects")]
        public async Task<IActionResult> GetMyProjects()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var projects = await _projectService.GetProjectsByUserAsync(userId);
            return Ok(projects);
        }

        // ---------------- UPDATE PROJECT ----------------
        [HttpPut("{projectId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> UpdateProject(int projectId, [FromBody] ProjectUpdateDto dto)
        {
            var updatedProject = await _projectService.UpdateProjectAsync(projectId, dto);
            if (updatedProject == null) return NotFound();
            return Ok(updatedProject);
        }

        // ---------------- DELETE PROJECT ----------------
        [HttpDelete("{projectId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeleteProject(int projectId)
        {
            var success = await _projectService.DeleteProjectAsync(projectId);
            if (!success) return NotFound();
            return NoContent();
        }

        // ---------------- ADD USER TO PROJECT ----------------
        [HttpPost("{projectId}/users/{userId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> AddUserToProject(int projectId, int userId)
        {
            var success = await _projectService.AddUserToProjectAsync(projectId, userId);
            if (!success) return BadRequest("Could not add user to project.");
            return Ok(new { Message = "User added to project successfully." });
        }

        // ---------------- REMOVE USER FROM PROJECT ----------------
        [HttpDelete("{projectId}/users/{userId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> RemoveUserFromProject(int projectId, int userId)
        {
            var success = await _projectService.RemoveUserFromProjectAsync(projectId, userId);
            if (!success) return BadRequest("Could not remove user from project.");
            return Ok(new { Message = "User removed from project successfully." });
        }
        [HttpGet("search")]
        public async Task<IActionResult> SearchProjects(
    [FromQuery] string? keyword)
        {
            var projects = await _projectService.SearchAsync(keyword);
            return Ok(projects);
        }

    }
}
