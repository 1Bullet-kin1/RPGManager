using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class Region
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int ContinentId { get; set; }

    public virtual Continent Continent { get; set; } = null!;

    public virtual ICollection<Location> Locations { get; set; } = new List<Location>();
}
