using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class TaskTimeline
{
    public int Id { get; set; }

    public int TaskId { get; set; }

    public int UserId { get; set; }

    public string Action { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public virtual TaskItem Task { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
