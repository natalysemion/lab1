using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1Football.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ClosedXML.Excel;
using System.Net;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Lab1Football.Controllers
{
    public class CountriesController : Controller
    {
        private readonly Lab1FootballContext _context;

        public CountriesController(Lab1FootballContext context)
        {
            _context = context;
        }
        public IActionResult IsNameExist(string Name)
        {
            bool exists = _context.Countries.Any(c => c.Name == Name);
            return Json(!exists);
        }


        // GET: Countries
        public async Task<IActionResult> Index()
        {
              return _context.Countries != null ? 
                          View(await _context.Countries.ToListAsync()) :
                          Problem("Entity set 'Lab1FootballContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // GET: Countries/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,WorldRating")] Country country)
        {

            if (ModelState.IsValid)
            {
                /*
                foreach(Country c in _context.Countries)
                if(country.Name == c.Name)
                {
                       // [Required(ErrorMessage = "існує")]
                        
                    return View(country);
                    
                }
                */
                    
                _context.Add(country);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            return View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,WorldRating")] Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(country);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CountryExists(country.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            var country = await _context.Countries
                .FirstOrDefaultAsync(m => m.Id == id);
            if (country == null)
            {
                return NotFound();
            }

            return View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var players = _context.Players.ToList();
            if (_context.Countries == null)
            {
                return Problem("Entity set 'Lab1FootballContext.Countries'  is null.");
            }
            var country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                foreach (var player in country.Players)
                {
                    _context.Players.Remove(player);
                }
                _context.Countries.Remove(country);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CountryExists(int id)
        {
          return (_context.Countries?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private Country GetOrCreateCountry(string countryName)
        {
            Country newCountry;
            //worksheet.Name - назва країни. Пробуємо знайти в БД, якщо відсутня, то створюємо нову Country;
            var c = (from cat in _context.Countries
                     where cat.Name.Contains(countryName)
                     select cat).ToList();
            if (c.Count > 0)
            {
                newCountry = c[0];
            }
            else
            {
                newCountry = new Country();
                newCountry.Name = countryName;
                //newCountry.Info = "from EXCEL";
                //додати в контекст
                _context.Countries.Add(newCountry);
            }
            return newCountry;
        }

        private Player CreatePlayer(IXLRow row, Country country)
        {
            Player player = new Player();
            player.Name = row.Cell(1).Value.ToString();
            int value, value1;
            if (Int32.TryParse(row.Cell(3).Value.ToString(), out value))
            {
                if(value < 0) {
                    throw new Exception("Ціна не може бути від'ємною. Комірка:  " + row.Cell(3).Address);
                }
                player.Price = value;
            }
            else
            {
                throw new Exception("Неправильний формат числа в комірці " + row.Cell(3).Address);
            }
            
            if (Int32.TryParse(row.Cell(4).Value.ToString(), out value1))
            {
                if (value1 < 0)
                {
                    throw new Exception("Номер не може бути від'ємним. Комірка:  " + row.Cell(4).Address);
                }
                player.Number = value1;
            }
            else
            {
                throw new Exception("Неправильний формат числа в комірці " + row.Cell(4).Address);
            }
            player.Country = country;
            player.CountryId = country.Id;
            if (row.Cell(6).Value.ToString().Length > 0)
            {
                Club club;
                var a = (from cl in _context.Clubs
                         where cl.Name.Contains(row.Cell(6).Value.ToString())
                         select cl).ToList();
                if (a.Count > 0)
                {
                    club = a[0];
                }
                else
                {
                    club = new Club();
                    club.Name = row.Cell(6).Value.ToString();

                    _context.Clubs.Add(club);
                }
                player.Club = club;
                player.ClubId = club.Id;

            }
            if (row.Cell(7).Value.ToString().Length > 0)
            {
                Position position;
                var a = (from pos in _context.Positions
                         where pos.Name.Contains(row.Cell(7).Value.ToString())
                         select pos).ToList();
                if (a.Count > 0)
                {
                    position = a[0];
                }
                else
                {
                    throw new Exception("Ви намагаєтеся завантажити дані з неправильними даними 'Позиція'.");
                }
                player.Position = position;
                player.PositionId = position.Id;
            }

            return player;
        }
    

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Import(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                if (fileExcel != null)
                {
                    using (var stream = new FileStream(fileExcel.FileName, FileMode.Create))
                    {
                    
                        await fileExcel.CopyToAsync(stream);
                        // using (XLWorkbook workBook = new XLWorkbook(stream.Name)) 
                        try
                        {
                            using (XLWorkbook workBook = new XLWorkbook(stream))
                            {
                                //перегляд усіх листів (в даному випадку країн)
                                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                                {
                                    var country = GetOrCreateCountry(worksheet.Name);
                                      //перегляд усіх рядків
                                    foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                    {
                                        var player = CreatePlayer(row, country);
                                        _context.Players.Add(player);
                                        await _context.SaveChangesAsync();
                                        
                                    }
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            ViewBag.Error = e.Message;
                            return View();
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        
        public ActionResult Export()
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var clubs = _context.Clubs.Include("Players").ToList();
                var positions = _context.Positions.Include("Players").ToList();
                var countries = _context.Countries.Include("Players").ToList();
                

                foreach (var c in countries)
                {
                    
                    var worksheet = workbook.Worksheets.Add(c.Name);
                    worksheet.Cell("A1").Value = "Ім'я";
                    worksheet.Cell("B1").Value = "Дата народження";
                    worksheet.Cell("C1").Value = "Ціна";
                    worksheet.Cell("D1").Value = "Номер";
                    worksheet.Cell("E1").Value = "Країна";
                    worksheet.Cell("F1").Value = "Клуб";
                    worksheet.Cell("G1").Value = "Позиція";
                    worksheet.Row(1).Style.Font.Bold = true;
                    var players = c.Players.ToList();
                     for (int i = 0; i < players.Count; i++)
                    {
                        worksheet.Cell(i + 2, 1).Value = players[i].Name;
                        worksheet.Cell(i + 2, 2).Value = players[i].DateOfBirth;
                        worksheet.Cell(i + 2, 3).Value = players[i].Price;
                        worksheet.Cell(i + 2, 4).Value = players[i].Number;
                        worksheet.Cell(i + 2, 5).Value = players[i].Country.Name; 
                        worksheet.Cell(i + 2, 6).Value = players[i].Club.Name;
                        worksheet.Cell(i + 2, 7).Value = players[i].Position.Name;
                       
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(),
                    "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"lab_football_countries_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
        
    }

}
