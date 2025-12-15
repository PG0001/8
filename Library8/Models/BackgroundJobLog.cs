using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class BackgroundJobLog
{
    public int Id { get; set; }

    public string? JobName { get; set; }

    public DateTime? RunTime { get; set; }

    public int? RecordsProcessed { get; set; }
}
