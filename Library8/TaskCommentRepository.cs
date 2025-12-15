using Library8.Models;
using Library8.Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Library8
{
    public class TaskCommentRepository : ITaskCommentRepository
    {
        private readonly DBContext _context;

        public TaskCommentRepository(DBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskComment comment)
        {
            await _context.TaskComments.AddAsync(comment);
        }

        public async Task<IEnumerable<TaskComment>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskComments
                .Where(c => c.TaskId == taskId)
                .OrderBy(c => c.CreatedOn)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
