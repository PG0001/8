namespace TaskManagementAPI.BackgroundJobs
{
    using Library8.Models;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using TaskManagementAPI.Services.Interfaces;

    public class TaskDueReminderWorker : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<TaskDueReminderWorker> _logger;

        public TaskDueReminderWorker(
            IServiceScopeFactory scopeFactory,
            ILogger<TaskDueReminderWorker> logger)
        {
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            _logger.LogInformation("TaskDueReminderWorker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await ProcessAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error in TaskDueReminderWorker");
                }

                // ⏰ Runs every 1 hour
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }

        private async Task ProcessAsync()
        {
            using var scope = _scopeFactory.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<DBContext>();
            var notificationService =
                scope.ServiceProvider.GetRequiredService<INotificationService>();

            var now = DateTime.UtcNow;
            var limit = now.AddHours(24);

            var dueTasks = await db.TaskItems
                .Where(t =>
                    t.DueDate >= now &&
                    t.DueDate <= limit &&
                    t.Status != "Done")
                .ToListAsync();

            int processedCount = 0;

            foreach (var task in dueTasks)
            {
                await notificationService.NotifyAsync(
                    task.AssignedTo,
                    "Task Due Reminder",
                    $"Task '{task.Title}' is due by {task.DueDate:dd MMM yyyy HH:mm}",
                    "Task"
                );

                processedCount++;
            }

            // ✅ Log execution (aligned with your entity)
            db.BackgroundJobLogs.Add(new BackgroundJobLog
            {
                JobName = "TaskDueReminderWorker",
                RunTime = DateTime.UtcNow,
                RecordsProcessed = processedCount
            });

            await db.SaveChangesAsync();
        }
    }

}
