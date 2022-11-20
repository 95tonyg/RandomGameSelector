using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RandomGameSelector.Data;
using RandomGameSelector.Models;

namespace RandomGameSelector.Controllers
{
    public class GamesController : Controller
    {
        private readonly RandomGameSelectorContext _context;

        public GamesController(RandomGameSelectorContext context)
        {
            _context = context;
        }

        public List<Genre> GetGenres()
        {
            return _context.Genre.ToList();
        }

        // GET: Games
        public async Task<IActionResult> ListPage()
        {
            ListPage listPage = new ListPage();
            listPage.Games = await _context.Game.ToListAsync();
            listPage.Genres = await _context.Genre.ToListAsync();

            return View(listPage);
        }

        // GET: Games/Details/{Id}
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Edit
        public IActionResult Create()
        {
            GameGenres gameGenres = new GameGenres();
            gameGenres.Genres = GetGenres();
            return View("Edit", gameGenres);
        }


        //GET: Games/CreateGenre
        public IActionResult CreateGenre()
        {
            return View("Genre");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> CreateGenre([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPage));
            }
            return View(genre);
        }

        // GET: Games/Edit/{Id}
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game.FindAsync(id);
            if (game == null)
            {
                return NotFound();
            }

            GameGenres gameGenres = new GameGenres();
            gameGenres.Game = game;
            gameGenres.Genres = GetGenres();
            return View(gameGenres);
        }

        // POST: Games/Edit/{Id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ListPage));
            }
            else if(id == 0 && game.Name != null)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPage));
            }
            GameGenres gameGenres = new GameGenres();
            gameGenres.Game = game;
            gameGenres.Genres = GetGenres();
            return View(gameGenres);
        }

        // GET: Games/Delete/{Id}
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Game == null)
            {
                return NotFound();
            }

            var game = await _context.Game
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            GameGenres gameGenres = new GameGenres();
            gameGenres.Game = game;
            gameGenres.Genres = GetGenres();
            return View("Edit", gameGenres);
        }

        // POST: Games/Delete/{Id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Game == null)
            {
                return Problem("Entity set 'RandomGameSelectorContext.Game'  is null.");
            }
            var game = await _context.Game.FindAsync(id);
            if (game != null)
            {
                _context.Game.Remove(game);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListPage));
        }

        private bool GameExists(int id)
        {
          return _context.Game.Any(e => e.Id == id);
        }
    }
}
