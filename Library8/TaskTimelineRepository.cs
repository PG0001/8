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
    public class TaskTimelineRepository : ITaskTimelineRepository
    {
        private readonly DBContext _context;

        public TaskTimelineRepository(DBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(TaskTimeline timeline)
        {
            await _context.TaskTimelines.AddAsync(timeline);
        }

        public async Task<IEnumerable<TaskTimeline>> GetByTaskIdAsync(int taskId)
        {
            return await _context.TaskTimelines
                .Where(t => t.TaskId == taskId)
                .OrderByDescending(t => t.CreatedOn)
                .ToListAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
