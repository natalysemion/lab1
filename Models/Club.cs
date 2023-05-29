using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1Football.Models;

public partial class Club
{
    public int Id { get; set; }
    [Display(Name = "Назва:")]
    public string Name { get; set; } = null!;

    [Display(Name = "Місце:")]
    public int Place { get; set; }
    [Display(Name = "Головний тренер:")]
    public int? HeadcoachId { get; set; }
    [Display(Name = "Інфо:")]
    public string? Info { get; set; }
    [Display(Name = "Кількість очок:")]
    public int Points { get; set; }
    [Display(Name = "Головний тренер:")]
    public virtual Headcoach? Headcoach { get; set; }

    public virtual ICollection<Player> Players { get; } = new List<Player>();
}
