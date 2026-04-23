using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Faction
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Alignment { get; set; }

    public string? Description { get; set; }

    public int? BaseLocationId { get; set; }

    public virtual Location? BaseLocation { get; set; }

    public virtual ICollection<Npc> Npcs { get; set; } = new List<Npc>();
}
