using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1Football.Models;

public partial class Headcoach
{
    public int Id { get; set; }

    [Display(Name = "Ім'я:")]
    public string Name { get; set; } = null!;
    [Display(Name = "Досягнення")]
    public string? Achievements { get; set; }

    public virtual ICollection<Club> Clubs { get; } = new List<Club>();
}
