using System;
using System.Collections.Generic;

namespace RPGManager.Models;

public partial class PinnedNpc
{
    public int Id { get; set; }

    public int NpcId { get; set; }

    public int Slot { get; set; }

    public virtual Npc Npc { get; set; } = null!;
}
