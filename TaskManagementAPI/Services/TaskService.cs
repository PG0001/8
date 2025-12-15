using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Threading.Tasks;
using TaskManagementAPI.common;
using TaskManagementAPI.Dtos;
using TaskManagementAPI.Hubs;
using TaskManagementAPI.Models;
using TaskManagementAPI.Services.Interfaces;
namespace TaskManagementAPI.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repo;

        private readonly INotificationService _notificationService;
        private readonly ITaskTimelineRepository _timelineRepo;
        private readonly IMemoryCache _cache;
        private readonly IHubContext<TaskHub> _taskHub;
        public TaskService(
      ITaskRepository repo,
      INotificationService notificationService,
      ITaskTimelineRepository timelineRepo,
      IMemoryCache cache,
      IHubContext<TaskHub> taskHub)
        {
            _repo = repo;
            _notificationService = notificationService;
            _timelineRepo = timelineRepo;
            _cache = cache;
            _taskHub = taskHub;
        }


        private async Task LogTimeline(
    int taskId,
    string action,
    int userId)
        {
            await _timelineRepo.AddAsync(new TaskTimeline
            {
                TaskId = taskId,
                Action = action,
                UserId = userId
            });

            await _timelineRepo.SaveChangesAsync();
        }

        // ---------------- CREATE TASK ----------------
        public async Task<Models.TaskItem> CreateTaskAsync(TaskCreateDto dto, int createdBy)
        {
            var task = new Library8.Models.TaskItem
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                AssignedTo = dto.AssignedTo,
                Priority = dto.Priority,
                Status = dto.Status,
                DueDate = dto.DueDate,
                CreatedOn = DateTime.UtcNow
            };

            await _repo.AddAsync(task);
            await _repo.SaveChangesAsync();
            await _notificationService.NotifyAsync(
    task.AssignedTo,
    "Task Assigned",
    $"You have been assigned a new task: {task.Title}",
    "Task"
);
            await LogTimeline(task.Id, "Task created", createdBy);


            _cache.Remove(CacheKeys.Dashboard);
            return MapToTaskDto(task);
        }

        // ---------------- GET TASK BY ID ----------------
        public async Task<Models.TaskItem?> GetTaskByIdAsync(int taskId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return null;
            return MapToTaskDto(task);
        }

        // ---------------- GET TASKS BY PROJECT ----------------
        public async Task<List<Models.TaskItem>> GetTasksByProjectAsync(int projectId)
        {
            var tasks = await _repo.GetTasksByProjectAsync(projectId);
            return tasks.Select(MapToTaskDto).ToList();
        }

        // ---------------- UPDATE TASK ----------------
        public async Task<Models.TaskItem?> UpdateTaskAsync(int taskId, TaskUpdateDto dto)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return null;

            task.Title = dto.Title ?? task.Title;
            task.Description = dto.Description ?? task.Description;
            task.AssignedTo = dto.AssignedTo ?? task.AssignedTo;
            task.Priority = dto.Priority ?? task.Priority;
            task.Status = dto.Status ?? task.Status;
            task.DueDate = dto.DueDate ?? task.DueDate;

            await _repo.SaveChangesAsync();
            await _taskHub.Clients
    .Group($"project-{task.ProjectId}")
    .SendAsync("TaskUpdated", new
    {
        TaskId = task.Id,
        Title = task.Title,
        Priority = task.Priority,
        Status = task.Status,
        DueDate = task.DueDate
    });

            _cache.Remove(CacheKeys.Dashboard);

            return MapToTaskDto(task);
        }

        // ---------------- DELETE TASK ----------------
        public async Task<bool> DeleteTaskAsync(int taskId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return false;

            _repo.Remove(task);
            await _repo.SaveChangesAsync();


            _cache.Remove(CacheKeys.Dashboard);
            return true;
        }


        // ---------------- CHANGE STATUS ----------------
        public async Task<bool> ChangeTaskStatusAsync(int taskId, string status,int userId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.Status = status;
            await _repo.SaveChangesAsync();
            await LogTimeline(task.Id, $"Status changed to {status}", userId);

            await _taskHub.Clients
    .Group($"project-{task.ProjectId}")
    .SendAsync("TaskStatusChanged", new
    {
        TaskId = task.Id,
        Status = status
    });


            _cache.Remove(CacheKeys.Dashboard);

            return true;
        }
        public async Task<IEnumerable<TaskCommentResponseDto>> GetTaskCommentsAsync(int taskId)
        {
            var comments = await _repo.GetCommentsByTaskAsync(taskId);

            return comments.Select(c => new TaskCommentResponseDto
            {
                UserId = c.UserId,
                Comment = c.CommentText,
                CreatedOn = c.CreatedOn
            });
        }
        public async Task<IEnumerable<Models.TaskItem>> GetTasksAssignedToUserAsync(int userId)
        {
            var tasks = await _repo.GetByAssignedUserAsync(userId);
            return tasks.Select(MapToTaskDto);
        }

        // ---------------- REASSIGN TASK ----------------
        public async Task<bool> ReassignTaskAsync(int taskId, int userId)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null) return false;

            task.AssignedTo = userId;
            await _repo.SaveChangesAsync();
            await LogTimeline(task.Id, $"Task reassigned to user {userId}", userId);
            await _taskHub.Clients
    .Group($"project-{task.ProjectId}")
    .SendAsync("TaskReassigned", new
    {
        TaskId = task.Id,
        AssignedTo = userId
    });
            await _notificationService.NotifyAsync(
    userId,
    "Task Assigned",
    $"You have been assigned task: {task.Title}",
    "Task"
);

            return true;
        }


        // ---------------- GET TASK TIMELINE ----------------
        public async Task<List<TaskTimeline>> GetTaskTimelineAsync(int taskId)
        {
            return await _repo.GetTaskTimelineAsync(taskId);
        }



        // ---------------- HELPER ----------------
        private TaskManagementAPI.Models.TaskItem MapToTaskDto(Library8.Models.TaskItem task)
        {
            return new TaskManagementAPI.Models.TaskItem
            {
                Id = task.Id,
                ProjectId = task.ProjectId,
                Title = task.Title,
                Description = task.Description,
                AssignedTo = task.AssignedTo,
                Priority = task.Priority,
                Status = task.Status,
                DueDate = task.DueDate,
                CreatedOn = task.CreatedOn
            };
        }

        public async Task<TaskCommentResponseDto> AddCommentAsync(
       int taskId, int userId, string comment)
        {
            var task = await _repo.GetByIdAsync(taskId);
            if (task == null)
                throw new Exception("Task not found");

            var entity = new Library8.Models.TaskComment
            {
                TaskId = taskId,
                UserId = userId,
                CommentText = comment
            };

            await _repo.AddCommentAsync(entity);

            await _repo.SaveChangesAsync();

            await LogTimeline(taskId, "Comment added", userId);

            await _taskHub.Clients
                .Group($"project-{task.ProjectId}")
                .SendAsync("TaskCommentAdded", new
                {
                    TaskId = taskId,
                    Comment = comment,
                    UserId = userId
                });

            return new TaskCommentResponseDto
            {
                UserId = userId,
                Comment = comment,
                CreatedOn = entity.CreatedOn
            };
        }



    }
}
