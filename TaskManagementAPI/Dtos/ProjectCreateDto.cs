namespace TaskManagementAPI.Dtos
{
    // Dtos/ProjectCreateDto.cs
    public class ProjectCreateDto
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }

    // Dtos/ProjectUpdateDto.cs
    public class ProjectUpdateDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? Status { get; set; }  // "Active" / "Completed"
    }

    // Dtos/ProjectDto.cs
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Status { get; set; } = "Active";
        public int CreatedBy { get; set; }
    }


}
