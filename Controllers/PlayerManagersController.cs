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
    public class PlayerManagersController : Controller
    {
        private readonly Lab1FootballContext _context;

        public PlayerManagersController(Lab1FootballContext context)
        {
            _context = context;
        }

        // GET: PlayerManagers
        public async Task<IActionResult> Index()
        {
            var lab1FootballContext = _context.PlayerManagers.Include(p => p.Manager).Include(p => p.Player);
            return View(await lab1FootballContext.ToListAsync());
        }

        // GET: PlayerManagers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.PlayerManagers == null)
            {
                return NotFound();
            }

            var playerManager = await _context.PlayerManagers
                .Include(p => p.Manager)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerManager == null)
            {
                return NotFound();
            }

            return View(playerManager);
        }

        // GET: PlayerManagers/Create
        public IActionResult Create()
        {
            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id");
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id");
            return View();
        }

        // POST: PlayerManagers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,ManagerId")] PlayerManager playerManager)
        {
            if (ModelState.IsValid)
            {
                _context.Add(playerManager);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id", playerManager.ManagerId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id", playerManager.PlayerId);
            return View(playerManager);
        }

        // GET: PlayerManagers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.PlayerManagers == null)
            {
                return NotFound();
            }

            var playerManager = await _context.PlayerManagers.FindAsync(id);
            if (playerManager == null)
            {
                return NotFound();
            }
            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id", playerManager.ManagerId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id", playerManager.PlayerId);
            return View(playerManager);
        }

        // POST: PlayerManagers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,ManagerId")] PlayerManager playerManager)
        {
            if (id != playerManager.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(playerManager);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerManagerExists(playerManager.Id))
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
            ViewData["ManagerId"] = new SelectList(_context.Managers, "Id", "Id", playerManager.ManagerId);
            ViewData["PlayerId"] = new SelectList(_context.Players, "Id", "Id", playerManager.PlayerId);
            return View(playerManager);
        }

        // GET: PlayerManagers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.PlayerManagers == null)
            {
                return NotFound();
            }

            var playerManager = await _context.PlayerManagers
                .Include(p => p.Manager)
                .Include(p => p.Player)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (playerManager == null)
            {
                return NotFound();
            }

            return View(playerManager);
        }

        // POST: PlayerManagers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.PlayerManagers == null)
            {
                return Problem("Entity set 'Lab1FootballContext.PlayerManagers'  is null.");
            }
            var playerManager = await _context.PlayerManagers.FindAsync(id);
            if (playerManager != null)
            {
                _context.PlayerManagers.Remove(playerManager);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerManagerExists(int id)
        {
          return (_context.PlayerManagers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
