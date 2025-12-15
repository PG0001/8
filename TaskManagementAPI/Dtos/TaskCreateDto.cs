namespace TaskManagementAPI.Dtos
{
    public class TaskCreateDto
    {
        public int ProjectId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public int AssignedTo { get; set; }
        public string Priority { get; set; } = "Low"; // Low/Medium/High
        public string Status { get; set; } = "Todo"; // Todo/InProgress/Review/Done
        public DateTime DueDate { get; set; }
    }

    // ---------------- UPDATE TASK ----------------
    public class TaskUpdateDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssignedTo { get; set; }
        public string? Priority { get; set; }
        public string? Status { get; set; }
        public DateTime? DueDate { get; set; }
    }

    // ---------------- STATUS UPDATE ----------------
    public class TaskStatusUpdateDto
    {
        public string Status { get; set; } = null!;
    }

    // ---------------- COMMENT ----------------
    public class TaskCommentDto
    {
        public string Comment { get; set; } = null!;
    }

}
