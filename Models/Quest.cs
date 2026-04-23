using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Quest
{
    public int Id { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public string? Status { get; set; }

    public int? LocationId { get; set; }

    public int? QuestGiverId { get; set; }

    public virtual Location? Location { get; set; }

    public virtual Npc? QuestGiver { get; set; }

    public virtual ICollection<QuestNpc> QuestNpcs { get; set; } = new List<QuestNpc>();
}
