using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class Notification
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string? Title { get; set; }

    public string? Message { get; set; }
    public bool IsRead { get; set; }

    public DateTime CreatedOn { get; set; }

    public virtual User User { get; set; } = null!;
}
