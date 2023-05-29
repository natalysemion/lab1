using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1Football.Models;

public partial class Manager
{
    public int Id { get; set; }
    [Display(Name = "Email:")]
    public string Name { get; set; } = null!;
    public string? Email { get; set; }

    public virtual ICollection<PlayerManager> PlayerManagers { get; } = new List<PlayerManager>();
}
