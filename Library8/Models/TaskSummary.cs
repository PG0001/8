using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class TaskSummary
{
    public int? TotalTasks { get; set; }

    public int? TodoCount { get; set; }

    public int? InProgressCount { get; set; }

    public int? ReviewCount { get; set; }

    public int? DoneCount { get; set; }
}
