using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1Football.Models;
using ClosedXML.Excel;
using Microsoft.AspNetCore.Authorization;

namespace Lab1Football.Controllers
{
    public class ClubsController : Controller
    {
        private readonly Lab1FootballContext _context;

        public ClubsController(Lab1FootballContext context)
        {
            _context = context;
        }

        // GET: Clubs
        public async Task<IActionResult> Index()
        {
            var lab1FootballContext = _context.Clubs.Include(c => c.Headcoach);
            return View(await lab1FootballContext.ToListAsync());
        }

        // GET: Clubs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .Include(c => c.Headcoach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // GET: Clubs/Create
        public IActionResult Create()
        {
            ViewData["HeadcoachId"] = new SelectList(_context.Headcoaches, "Id", "Name");
            return View();
        }

        // POST: Clubs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Place,HeadcoachId,Info,Points")] Club club)
        {
            if (ModelState.IsValid)
            {
                _context.Add(club);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["HeadcoachId"] = new SelectList(_context.Headcoaches, "Id", "Id", club.HeadcoachId);
            return View(club);
        }

        // GET: Clubs/Edit/5
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs.FindAsync(id);
            if (club == null)
            {
                return NotFound();
            }
            /*
            if (!User.IsInRole("admin"))
            {
                return View("AccessDenied");
            }
            */
            ViewData["HeadcoachId"] = new SelectList(_context.Headcoaches, "Id", "Name");
            return View(club);
        }

        // POST: Clubs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Place,HeadcoachId,Info,Points")] Club club)
        {
            if (id != club.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(club);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ClubExists(club.Id))
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
            ViewData["HeadcoachId"] = new SelectList(_context.Headcoaches, "Id", "Id", club.HeadcoachId);
            return View(club);
        }

        // GET: Clubs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Clubs == null)
            {
                return NotFound();
            }

            var club = await _context.Clubs
                .Include(c => c.Headcoach)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (club == null)
            {
                return NotFound();
            }

            return View(club);
        }

        // POST: Clubs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Clubs == null)
            {
                return Problem("Entity set 'Lab1FootballContext.Clubs'  is null.");
            }
            var club = await _context.Clubs.FindAsync(id);
            if (club != null)
            {
                var players = _context.Players.Where(p => p.ClubId == club.Id).ToList();
                var club0 = _context.Clubs.Where(p => p.Name == "вільний агент").ToList();
                
                foreach(var p in players)
                {
                    p.Club = club0[0];
                    p.ClubId = club0[0].Id;
                }
                _context.Clubs.Remove(club);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ClubExists(int id)
        {
          return (_context.Clubs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        /*
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
                        using (XLWorkbook workBook = new XLWorkbook(stream, XLEventTracking.Disabled))
                        {
                            //перегляд усіх листів (в даному випадку категорій)
                            foreach (IXLWorksheet worksheet in workBook.Worksheets)
                            {
                                //worksheet.Name - назва категорії. Пробуємо знайти в БД, якщо відсутня, то створюємо нову Category newcat;
                                Club newcl;
                                var c = (from cl in _context.Clubs
                                         where cl.Name.Contains(worksheet.Name)
                                         select cl).ToList();
                                if (c.Count > 0)
                                {
                                    newcl = c[0];
                                }
                                else
                                {
                                    newcl = new Club();
                                    newcl.Name = worksheet.Name;
                                    newcl.Info = "from EXCEL";
                                    //додати в контекст
                                    _context.Clubs.Add(newcl);
                                }
                                /*
                                //перегляд усіх рядків
                                foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                                {
                                    try
                                    {
                                        Book book = new Book();
                                        book.Name = row.Cell(1).Value.ToString();
                                        book.Info = row.Cell(6).Value.ToString();
                                        book.Category = newcat;
                                        _context.Books.Add(book);
                                        //у разі наявності автора знайти його, у разі відсутності - додати
                                        for (int i = 2; i <= 5; i++)
                                        {
                                            if (row.Cell(i).Value.ToString().Length > 0)
                                            {
                                                Author author;
                                                var a = (from aut in _context.Authors
                                                         where aut.Name.Contains(row.Cell(i).Value.ToString())
                                                         select aut).ToList();
                                                if (a.Count > 0)
                                                {
                                                    author = a[0];
                                                }
                                                else
                                                {
                                                    author = new Author();
                                                    author.Name = row.Cell(i).Value.ToString();
                                                    author.Info = "from EXCEL";
                                                    //додати в контекст
                                                    _context.Add(author);
                                                }
                                                AuthorBook ab = new AuthorBook();
                                                ab.Book = book;
                                                ab.Author = author;
                                                _context.AuthorBooks.Add(ab);
                                            }
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        //logging самостійно :)
                                    }
                                }
                                
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
        */
    }
}
