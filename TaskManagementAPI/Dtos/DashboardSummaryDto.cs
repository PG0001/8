namespace TaskManagementAPI.Dtos
{
    public class DashboardSummaryDto
    {
        public int TotalProjects { get; set; }
        public int TotalTasks { get; set; }
        public int OverdueTasks { get; set; }
        public int UpcomingDeadlines { get; set; }

        public Dictionary<string, int> TasksByStatus { get; set; } = new();
        public Dictionary<string, int> TasksByPriority { get; set; } = new();
        public Dictionary<string, int> UserWorkload { get; set; } = new();
    }

}
