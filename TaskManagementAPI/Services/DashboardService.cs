namespace TaskManagementAPI.Services
{
    using Library8.Models;
    using Library8.Models.Interfaces;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using TaskManagementAPI.Dtos;
    using TaskManagementAPI.Services.Interfaces;

    public class DashboardService : IDashboardService
    {
        private readonly DBContext _context;
        private readonly IMemoryCache _cache;

        private const string CACHE_KEY = "dashboard_summary";

        public DashboardService(DBContext context, IMemoryCache cache)
        {
            _context = context;
            _cache = cache;
        }
        private async Task<DashboardSummaryDto> BuildDashboardAsync()
        {
            var now = DateTime.UtcNow;

            var totalProjects = await _context.Projects.CountAsync();
            var totalTasks = await _context.TaskItems.CountAsync();

            var overdueTasks = await _context.TaskItems
                .CountAsync(t => t.DueDate < now && t.Status != "Done");

            var upcomingDeadlines = await _context.TaskItems
                .CountAsync(t => t.DueDate >= now && t.DueDate <= now.AddDays(7));

            var tasksByStatus = await _context.TaskItems
                .GroupBy(t => t.Status)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);

            var tasksByPriority = await _context.TaskItems
                .GroupBy(t => t.Priority)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key, x => x.Count);

            var userWorkload = await _context.TaskItems
                .Include(t => t.AssignedToNavigation)
                .GroupBy(t => t.AssignedToNavigation.FullName)
                .Select(g => new { User = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.User, x => x.Count);

            var summary = new DashboardSummaryDto
            {
                TotalProjects = totalProjects,
                TotalTasks = totalTasks,
                OverdueTasks = overdueTasks,
                UpcomingDeadlines = upcomingDeadlines,
                TasksByStatus = tasksByStatus,
                TasksByPriority = tasksByPriority,
                UserWorkload = userWorkload
            };
            return summary;

        }

        public async Task<DashboardSummaryDto> GetSummaryAsync()
        {
            // ✅ READ CACHE
            if (_cache.TryGetValue(CACHE_KEY, out DashboardSummaryDto cached))
                return cached;

            // ❌ Cache miss → hit DB
            var data = await BuildDashboardAsync();



            // ✅ WRITE CACHE
            _cache.Set(
                CACHE_KEY,
                data,
                TimeSpan.FromMinutes(10)
            );

            return data;
        }
    }

}

