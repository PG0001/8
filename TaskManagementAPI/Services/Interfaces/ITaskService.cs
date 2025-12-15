using TaskManagementAPI.Dtos;
using TaskManagementAPI.Models; // API-facing DTOs / simplified models
using Library8.Models;          // EF Core database models

namespace TaskManagementAPI.Services.Interfaces
{
    public interface ITaskService
    {
        // ---------------- CREATE TASK ----------------
        // Returns API model TaskItem
        Task<Models.TaskItem> CreateTaskAsync(TaskCreateDto dto, int createdBy);

        // ---------------- GET TASK BY ID ----------------
        // Returns API model TaskItem or null
        Task<Models.TaskItem?> GetTaskByIdAsync(int taskId);

        // ---------------- GET TASKS BY PROJECT ----------------
        // Returns list of API model TaskItem
        Task<List<Models.TaskItem>> GetTasksByProjectAsync(int projectId);

        // ---------------- UPDATE TASK ----------------
        // Returns updated API model TaskItem or null
        Task<Models.TaskItem?> UpdateTaskAsync(int taskId, TaskUpdateDto dto);

        // ---------------- DELETE TASK ----------------
        Task<bool> DeleteTaskAsync(int taskId);

        // ---------------- CHANGE STATUS ----------------
        Task<bool> ChangeTaskStatusAsync(int taskId, string status, int userId);

        // ---------------- REASSIGN TASK ----------------
        Task<bool> ReassignTaskAsync(int taskId, int userId);

        // ---------------- ADD COMMENT ----------------
        // Returns API model TaskComment
        Task<TaskCommentResponseDto> AddCommentAsync(int taskId, int userId, string commentText);

        // ---------------- GET TASK TIMELINE ----------------
        // Returns list of database model TaskTimeline
        Task<List<TaskTimeline>> GetTaskTimelineAsync(int taskId);
        Task<IEnumerable<TaskCommentResponseDto>> GetTaskCommentsAsync(int taskId);
        Task<IEnumerable<Models.TaskItem>> GetTasksAssignedToUserAsync(int userId);

    }
}
