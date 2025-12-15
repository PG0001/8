using System;
using System.Collections.Generic;

namespace TaskManagementAPI.Models;

public class TaskItem
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int? AssignedTo { get; set; }
    public string Priority { get; set; } = "Medium";
    public string Status { get; set; } = "Todo";
    public DateTime? DueDate { get; set; }
    public DateTime? CreatedOn
    {
        get; set;
    }
}
