using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Note
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Content { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? LinkedType { get; set; }

    public int? LinkedId { get; set; }
}
