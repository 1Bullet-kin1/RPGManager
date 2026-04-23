using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class World
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Continent> Continents { get; set; } = new List<Continent>();
}
