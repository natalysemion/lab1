using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Lab1Football.Models;

public partial class Player
{
    public int Id { get; set; }
    [Display(Name = "Ім'я")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]

    public string Name { get; set; } = null!;
    [Display(Name = "Країна")]
    public int CountryId { get; set; }
    [Display(Name = "Клуб")]
    public int ClubId { get; set; }
 
    [Display(Name = "Дата народження")]
    public DateTime? DateOfBirth { get; set; }
    [Display(Name = "Ціна млн$")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public int Price { get; set; }
    [Display(Name = "Позиція")]
    public int PositionId { get; set; }
    [Display(Name = "Номер")]
    [Required(ErrorMessage = "Поле не повинно бути порожнім")]
    public int Number { get; set; }
    [Display(Name = "Менеджер")]
    public int? ManagerId { get; set; }
    [Display(Name = "Клуб")]
    public virtual Club Club { get; set; } = null!;
    [Display(Name = "Країна")]
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<PlayerManager> PlayerManagers { get; } = new List<PlayerManager>();
    [Display(Name = "Позиція")]
    public virtual Position Position { get; set; } = null!;
}
