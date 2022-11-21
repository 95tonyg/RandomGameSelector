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

        private List<Genre> GetGenres()
        {
            return _context.Genre.ToList();
        }

        public async Task<IActionResult> GenresListPage()
        {

            return View(await _context.Genre.ToListAsync());
        }

        // GET: Games
        public async Task<IActionResult> ListPage()
        {
            ListPage listPage = new ListPage();
            listPage.Games = await _context.Game.ToListAsync();
            listPage.Genres = await _context.Genre.ToListAsync();
            listPage.GameGenres = new List<string>();
            List <GameGenre> gameGenres = await _context.GameGenre.ToListAsync();

            foreach (var game in listPage.Games)
            {
                List<int> genreIds = gameGenres.Where(x => x.GameId == game.Id).Select(x => x.GenreId).ToList();
                List<string> genreString = new List<string>();
                foreach(var genre in listPage.Genres)
                {
                    if (genreIds.Contains(genre.Id))
                    {
                        genreString.Add(genre.Name);
                    }
                }
                if(genreString.Count > 0)
                {
                    listPage.GameGenres.Add(string.Join(", ", genreString));
                }
                else
                {
                    listPage.GameGenres.Add("");
                }
            }

            return View("ListGamesPage", listPage);
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
            GameEditViewModel gameGenres = new GameEditViewModel();
            gameGenres.AllGenres = GetGenres();
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
                return RedirectToAction(nameof(GenresListPage));
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

            GameEditViewModel gameGenres = new GameEditViewModel();
            gameGenres.Game = game;
            gameGenres.AllGenres = GetGenres();
            gameGenres.MatchedGenreIds = _context.GameGenre.Where(x => x.GameId == id).Select(x => x.GenreId).ToList();

            return View(gameGenres);
        }

        // POST: Games/Edit/{Id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string name, int[] genres)
        {
            Game game = new Game();
            game.Id = id;
            game.Name = name;

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
                    UpdateGameGenres(game.Id, genres);
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
                UpdateGameGenres(game.Id, genres);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPage));
            }
            GameEditViewModel gameGenres = new GameEditViewModel();
            gameGenres.Game = game;
            gameGenres.AllGenres = GetGenres();
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

            GameEditViewModel gameGenres = new GameEditViewModel();
            gameGenres.Game = game;
            gameGenres.AllGenres = GetGenres();
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

        private async void UpdateGameGenres(int gameId, int[] gameGenreIds)
        {
            List<GameGenre> matchedGameGenres = _context.GameGenre.Where(x => x.GameId == gameId).ToList();

            List<int> gameGenreIdsList = gameGenreIds.ToList();

            foreach(int genreId in gameGenreIdsList)
            {
                if (matchedGameGenres.Where(x => x.GenreId == genreId).Count() == 0)
                {                   
                    _context.GameGenre.Add(new GameGenre(gameId, genreId));
                }
                else
                {
                    matchedGameGenres.RemoveAll(x => x.GenreId == genreId);
                }
            }

            //If any matched GameGenres remain, then they haven't been selected. Therefore we must delete them.
            if(matchedGameGenres.Count() > 0)
            {
                foreach(GameGenre gameGenre in matchedGameGenres)
                {
                    _context.GameGenre.Remove(gameGenre);
                }
            }
        }

        private List<GameGenre> GetGameGenres(int gameId)
        {
            return _context.GameGenre.Where(x => x.GameId == gameId).ToList(); 
        }
    }
}
