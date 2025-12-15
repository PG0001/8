using Library8;
using Library8.Models;
using Library8.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
namespace Library8
{
    public class TaskRepository : ITaskRepository
    {
        private readonly DBContext _context;

        public TaskRepository(DBContext context)
        {
            _context = context;
        }

        // ---------------- TASK CRUD ----------------
        public async Task AddAsync(TaskItem task)
        {
            await _context.TaskItems.AddAsync(task);
        }

        public async Task<TaskItem?> GetByIdAsync(int taskId)
        {
            return await _context.TaskItems
                .Include(t => t.TaskComments)
                .Include(t => t.TaskTimelines)
                .FirstOrDefaultAsync(t => t.Id == taskId);
        }

        public async Task<List<TaskItem>> GetTasksByProjectAsync(int projectId)
        {
            return await _context.TaskItems
                .Where(t => t.ProjectId == projectId)
                .Include(t => t.TaskComments)
                .Include(t => t.TaskTimelines)
                .ToListAsync();
        }

        public void Remove(TaskItem task)
        {
            _context.TaskTimelines.RemoveRange(_context.TaskTimelines.Where(t => t.TaskId == task.Id));
            _context.TaskItems.Remove(task);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        // ---------------- COMMENTS ----------------
        public async Task AddCommentAsync(TaskComment comment)
        {
            await _context.TaskComments.AddAsync(comment);
        }

        public async Task<List<TaskComment>> GetCommentsByTaskAsync(int taskId)
        {
            return await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .Include(c => c.User)
                .ToListAsync();
        }

        // ---------------- TIMELINE ----------------
        public async Task<List<TaskTimeline>> GetTaskTimelineAsync(int taskId)
        {
            return await _context.TaskTimelines
                .Where(t => t.TaskId == taskId)
                .OrderBy(t => t.CreatedOn)
                .ToListAsync();
        }
        public async Task<IEnumerable<TaskItem>> GetByAssignedUserAsync(int userId)
        {
            return await _context.TaskItems
                .Where(t => t.AssignedTo == userId)
                .ToListAsync();
        }

    }
}