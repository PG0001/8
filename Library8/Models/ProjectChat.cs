using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class ProjectChat
{
    public int Id { get; set; }

    public int ProjectId { get; set; }

    public int UserId { get; set; }

    public string Message { get; set; }

    public string? FileUrl { get; set; }

    public DateTime SentOn { get; set; }

    public virtual Project Project { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
