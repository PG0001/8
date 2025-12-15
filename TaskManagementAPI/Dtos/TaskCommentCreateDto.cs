namespace TaskManagementAPI.Dtos
{
    public class TaskCommentCreateDto
    {
        public string Comment { get; set; } = null!;
    }
    public class TaskCommentResponseDto
    {
        public int UserId { get; set; }
        public string Comment { get; set; } = null!;
        public DateTime CreatedOn { get; set; }
    }

}
