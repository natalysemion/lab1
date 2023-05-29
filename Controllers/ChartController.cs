using Lab1Football.Models;
using Microsoft.AspNetCore.Mvc;

namespace Database.Controllers
{
    [Route("api/Chart")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly Lab1FootballContext _context;
        public ChartController(Lab1FootballContext context)
        {
            _context = context;
        }

        [HttpGet("JsonData")]
        public JsonResult JsonData()
        {
            var countries = _context.Countries.ToList();
            List<object> list = new List<object> { new object[] { "Country", "Players" } };
            foreach (var c in countries)
            {
                list.Add(new object[] { c.Name, _context.Players.Where(x => x.Country.Id == c.Id).Count() });
            }
            return new JsonResult(list);
        }
        [HttpGet("JsonDataClubs")]
        public JsonResult JsonDataClubs()
        {
            var countries = _context.Clubs.ToList();
            List<object> list = new List<object> { new object[] { "Club", "Players" } };
            foreach (var c in countries)
            {
                list.Add(new object[] { c.Name, _context.Players.Where(x => x.Club.Id == c.Id).Count() });
            }
            return new JsonResult(list);
        }
    }
}