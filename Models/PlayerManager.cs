using System;
using System.Collections.Generic;

namespace Lab1Football.Models;

public partial class PlayerManager
{
    public int Id { get; set; }

    public int PlayerId { get; set; }

    public int ManagerId { get; set; }

    public virtual Manager Manager { get; set; } = null!;

    public virtual Player Player { get; set; } = null!;
}
