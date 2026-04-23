using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Npcrelation
{
    public int Id { get; set; }

    public int NpcId1 { get; set; }

    public int NpcId2 { get; set; }

    public string? RelationType { get; set; }

    public string? Description { get; set; }

    public virtual Npc NpcId1Navigation { get; set; } = null!;

    public virtual Npc NpcId2Navigation { get; set; } = null!;
}
