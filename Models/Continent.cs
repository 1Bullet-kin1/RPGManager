using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Continent
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int WorldId { get; set; }

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    public virtual World World { get; set; } = null!;
}
