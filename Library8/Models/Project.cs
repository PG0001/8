using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class Project
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public string Status { get; set; } = null!;

    public int CreatedBy { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual User CreatedByNavigation { get; set; } = null!;

    public virtual ICollection<ProjectChat> ProjectChats { get; set; } = new List<ProjectChat>();

    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

    public virtual ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();
}
