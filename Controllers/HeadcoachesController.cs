using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Lab1Football.Models;

namespace Lab1Football.Controllers
{
    public class HeadcoachesController : Controller
    {
        private readonly Lab1FootballContext _context;

        public HeadcoachesController(Lab1FootballContext context)
        {
            _context = context;
        }

        // GET: Headcoaches
        /*
        public async Task<IActionResult> Index()
        {
              return _context.Headcoaches != null ? 
                          View(await _context.Headcoaches.ToListAsync()) :
                          Problem("Entity set 'Lab1FootballContext.Headcoaches'  is null.");
        }
        */
        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewData["CurrentFilter"] = searchString;
            var hcoaches = from c in _context.Headcoaches
                           select c;
            if (!String.IsNullOrEmpty(searchString))
            {
                hcoaches = hcoaches.Where(c => c.Name.Contains(searchString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    hcoaches = hcoaches.OrderByDescending(c => c.Name);
                    break;
                default:
                    hcoaches = hcoaches.OrderBy(c=> c.Name);
                    break;
            }
            int pageSize = 3;
            return View(await hcoaches.AsNoTracking().ToListAsync());
        }

        // GET: Headcoaches/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Headcoaches == null)
            {
                return NotFound();
            }

            var headcoach = await _context.Headcoaches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headcoach == null)
            {
                return NotFound();
            }

            return View(headcoach);
        }

        // GET: Headcoaches/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Headcoaches/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Achievements")] Headcoach headcoach)
        {
            if (ModelState.IsValid)
            {
                _context.Add(headcoach);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(headcoach);
        }

        // GET: Headcoaches/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Headcoaches == null)
            {
                return NotFound();
            }

            var headcoach = await _context.Headcoaches.FindAsync(id);
            if (headcoach == null)
            {
                return NotFound();
            }
            return View(headcoach);
        }

        // POST: Headcoaches/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Achievements")] Headcoach headcoach)
        {
            if (id != headcoach.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(headcoach);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HeadcoachExists(headcoach.Id))
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
            return View(headcoach);
        }

        // GET: Headcoaches/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Headcoaches == null)
            {
                return NotFound();
            }

            var headcoach = await _context.Headcoaches
                .FirstOrDefaultAsync(m => m.Id == id);
            if (headcoach == null)
            {
                return NotFound();
            }

            return View(headcoach);
        }

        // POST: Headcoaches/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Headcoaches == null)
            {
                return Problem("Entity set 'Lab1FootballContext.Headcoaches'  is null.");
            }
            var headcoach = await _context.Headcoaches.FindAsync(id);
            if (headcoach != null)
            {
                _context.Headcoaches.Remove(headcoach);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HeadcoachExists(int id)
        {
          return (_context.Headcoaches?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
