namespace TaskManagementAPI.Models
{
    public class TaskComment
    {
        public int Id { get; set; }
        public int TaskId { get; set; }
        public int UserId { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    }

}
