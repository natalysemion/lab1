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
    public class PlayersController : Controller
    {
        private readonly Lab1FootballContext _context;

        public PlayersController(Lab1FootballContext context)
        {
            _context = context;
        }
        // GET: Players
        public async Task<IActionResult> Index1(int? id, string? name)
        {
          
            if(id == null) return RedirectToAction("Positions", "Index");
            ViewBag.PositionId = id;
            ViewBag.PositionName = name;
            var playersByPosition = _context.Players.Where(b => b.PositionId == id).Include(b => b.Position);

            return View(await playersByPosition.ToListAsync());
           
        }

        public async Task<IActionResult> Index(int? id, string? name)
        {
            var lab1FootballContext = _context.Players.Include(p => p.Club).Include(p => p.Country).Include(p => p.Position);
            return View(await lab1FootballContext.ToListAsync());
           
        }
        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Club)
                .Include(p => p.Country)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // GET: Players/Create
        public IActionResult Create()
        {
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View();
        }
      
        public async Task<IActionResult> AddProduct(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }
            var products = await _context.Players.ToListAsync();
            ViewBag.CategoryId = id;
            return View(products);
        }
          [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,CountryId,ClubId,DateOfBirth,Price,PositionId,Number,ManagerId")] Player player)
        {
          //  try
            
                if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Id", player.ClubId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", player.CountryId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", player.PositionId);
            
            
            return View(player);
        }

        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return NotFound();
            }
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Name");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Name");
            return View(player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,CountryId,ClubId,DateOfBirth,Price,PositionId,Number,ManagerId")] Player player)
        {
            if (id != player.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.Id))
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
            ViewData["ClubId"] = new SelectList(_context.Clubs, "Id", "Id", player.ClubId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Id", player.CountryId);
            ViewData["PositionId"] = new SelectList(_context.Positions, "Id", "Id", player.PositionId);
            return View(player);
        }

        // GET: Players/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Players == null)
            {
                return NotFound();
            }

            var player = await _context.Players
                .Include(p => p.Club)
                .Include(p => p.Country)
                .Include(p => p.Position)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (player == null)
            {
                return NotFound();
            }

            return View(player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Players == null)
            {
                return Problem("Entity set 'Lab1FootballContext.Players'  is null.");
            }
            var player = await _context.Players.FindAsync(id);
            if (player != null)
            {
                _context.Players.Remove(player);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
          return (_context.Players?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
