using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class QuestNpc
{
    public int Id { get; set; }

    public int QuestId { get; set; }

    public int NpcId { get; set; }

    public string? Role { get; set; }

    public virtual Npc Npc { get; set; } = null!;

    public virtual Quest Quest { get; set; } = null!;
}
