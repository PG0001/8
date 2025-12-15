using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class User
{
    public int Id { get; set; }

    public string FullName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public int RoleId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual ICollection<File> Files { get; set; } = new List<File>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<ProjectChat> ProjectChats { get; set; } = new List<ProjectChat>();

    public virtual ICollection<ProjectUser> ProjectUsers { get; set; } = new List<ProjectUser>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<TaskComment> TaskComments { get; set; } = new List<TaskComment>();

    public virtual ICollection<TaskItem> TaskItems { get; set; } = new List<TaskItem>();

    public virtual ICollection<TaskTimeline> TaskTimelines { get; set; } = new List<TaskTimeline>();
}
