using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Npc
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Race { get; set; }

    public string? Class { get; set; }

    public int? Level { get; set; }

    public string? Alignment { get; set; }

    public string? Description { get; set; }

    public int? LocationId { get; set; }

    public int? FactionId { get; set; }

    public virtual Faction? Faction { get; set; }

    public virtual Location? Location { get; set; }

    public virtual ICollection<Npcrelation> NpcrelationNpcId1Navigations { get; set; } = new List<Npcrelation>();

    public virtual ICollection<Npcrelation> NpcrelationNpcId2Navigations { get; set; } = new List<Npcrelation>();

    public virtual ICollection<QuestNpc> QuestNpcs { get; set; } = new List<QuestNpc>();

    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();
}
