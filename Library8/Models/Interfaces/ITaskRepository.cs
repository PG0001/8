using Library8.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library8.Models.Interfaces
{
    public interface ITaskRepository
    {
        // ---------------- TASK CRUD ----------------
        Task AddAsync(TaskItem task);
        Task<TaskItem?> GetByIdAsync(int taskId);
        Task<List<TaskItem>> GetTasksByProjectAsync(int projectId);
        void Remove(TaskItem task);

        Task SaveChangesAsync();

        // ---------------- COMMENTS ----------------
        Task AddCommentAsync(TaskComment comment);
        Task<List<TaskComment>> GetCommentsByTaskAsync(int taskId);

        // ---------------- TIMELINE ----------------
        Task<List<TaskTimeline>> GetTaskTimelineAsync(int taskId);
        Task<IEnumerable<TaskItem>> GetByAssignedUserAsync(int userId);
        //Task AddTimelineAsync(TaskTimeline timeline);


    }
}
