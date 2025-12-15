using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class TaskItem
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public int AssignedTo { get; set; }

    public string Priority { get; set; } = null!;

    public string Status { get; set; } = null!;

    public DateTime? DueDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual User? AssignedToNavigation { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<TaskTimeline> TaskTimelines { get; set; } = new List<TaskTimeline>();
}
