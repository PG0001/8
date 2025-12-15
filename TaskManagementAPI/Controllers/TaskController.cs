using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Interfaces;

namespace TaskManagementAPI.Controllers
{
    [ApiController]
    [Route("api/tasks")]
    [Authorize]
    public class TaskController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TaskController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        // ---------------- CREATE TASK ----------------
        [HttpPost]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> CreateTask([FromBody] TaskCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var task = await _taskService.CreateTaskAsync(dto, userId);
            return Ok(task);
        }

        // ---------------- GET TASK BY ID ----------------
        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetTaskById(int taskId)
        {
            var task = await _taskService.GetTaskByIdAsync(taskId);
            if (task == null) return NotFound();

            return Ok(task);
        }

        // ---------------- GET TASKS BY PROJECT ----------------
        [HttpGet("project/{projectId}")]
        public async Task<IActionResult> GetTasksByProject(int projectId)
        {
            var tasks = await _taskService.GetTasksByProjectAsync(projectId);
            return Ok(tasks);
        }

        // ---------------- UPDATE TASK ----------------
        [HttpPut("{taskId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> UpdateTask(int taskId, [FromBody] TaskUpdateDto dto)
        {
            var updatedTask = await _taskService.UpdateTaskAsync(taskId, dto);
            if (updatedTask == null) return NotFound();

            return Ok(updatedTask);
        }

        // ---------------- DELETE TASK ----------------
        [HttpDelete("{taskId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> DeleteTask(int taskId)
        {
            var success = await _taskService.DeleteTaskAsync(taskId);
            if (!success) return NotFound();

            return NoContent();
        }

        // ---------------- CHANGE TASK STATUS ----------------
        [HttpPatch("{taskId}/status")]
        public async Task<IActionResult> ChangeStatus(
            int taskId,
            [FromBody] TaskStatusUpdateDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var success = await _taskService.ChangeTaskStatusAsync(
                taskId,
                dto.Status,
                userId
            );

            if (!success) return BadRequest("Unable to update task status.");

            return Ok(new { message = "Task status updated successfully" });
        }

        // ---------------- REASSIGN TASK ----------------
        [HttpPatch("{taskId}/reassign/{userId}")]
        [Authorize(Roles = "Admin,ProjectManager")]
        public async Task<IActionResult> ReassignTask(int taskId, int userId)
        {
            var success = await _taskService.ReassignTaskAsync(taskId, userId);
            if (!success) return BadRequest("Unable to reassign task.");

            return Ok(new { message = "Task reassigned successfully" });
        }

        // ---------------- ADD COMMENT ----------------
        [HttpPost("{taskId}/comments")]
        public async Task<IActionResult> AddComment(
            int taskId,
            [FromBody] TaskCommentCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);

            var comment = await _taskService.AddCommentAsync(
                taskId, userId, dto.Comment);

            return Ok(comment);
        }

        // ---------------- GET TASK TIMELINE ----------------
        [HttpGet("{taskId}/timeline")]
        public async Task<IActionResult> GetTaskTimeline(int taskId)
        {
            var timeline = await _taskService.GetTaskTimelineAsync(taskId);
            return Ok(timeline);
        }
        [HttpGet("{taskId}/comments")]
        public async Task<IActionResult> GetComments(int taskId)
        {
            var comments = await _taskService.GetTaskCommentsAsync(taskId);
            return Ok(comments);
        }
        [HttpGet("my")]
        public async Task<IActionResult> GetMyTasks()
        {
            var userId = int.Parse(User.FindFirst("UserId")!.Value);
            var tasks = await _taskService.GetTasksAssignedToUserAsync(userId);
            return Ok(tasks);
        }
        [HttpPost("{taskId}/attachments")]
        public async Task<IActionResult> UploadAttachment(
    int taskId, IFormFile file)
        {
            return Ok(); // implemented later in File module
        }


    }
}
