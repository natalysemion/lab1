/*using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Web.CodeGeneration.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;


namespace Lab1Football.Models;


    public partial class Country
    {
    private readonly Lab1FootballContext _context;
    public int Id { get; set; }

        // [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        
        [Display(Name = "Країна")]
        [Required(ErrorMessage = "Поле не повинно бути порожнім")]
        [Remote("IsNameExist", "Countries", ErrorMessage = "Країна з цією назвою вже існує")]
        public string Name { get; set; } = null!;

        [Display(Name = "Місце у рейтингу:")]
        public int? WorldRating { get; set; }

        public virtual ICollection<Player> Players { get; } = new List<Player>();

    

}
