using System;
using System.Collections.Generic;

namespace Library8.Models;

public partial class File
{
    public int Id { get; set; }

    public string? FileName { get; set; }

    public string? FilePath { get; set; }

    public int UploadedBy { get; set; }

    public DateTime UploadedOn { get; set; }

    public int RelatedEntityId { get; set; }

    public string EntityType { get; set; } = null!;

    public virtual User UploadedByNavigation { get; set; } = null!;
}
