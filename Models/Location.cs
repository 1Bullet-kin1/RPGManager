using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Location
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Type { get; set; }

    public string? Description { get; set; }

    public int RegionId { get; set; }

    public virtual ICollection<Faction> Factions { get; set; } = new List<Faction>();
    public virtual ICollection<Faction> PresentFactions { get; set; } = new List<Faction>(); 

    public virtual ICollection<Npc> Npcs { get; set; } = new List<Npc>();

    public virtual ICollection<Quest> Quests { get; set; } = new List<Quest>();

    public virtual Region Region { get; set; } = null!;
}
