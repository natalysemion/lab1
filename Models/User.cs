using Microsoft.AspNetCore.Identity;

namespace Lab1Football.Models
{
    public class User: IdentityUser
    {
        public int Year { get; set; }
    }
}
