using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RandomGameSelector.Data;
using RandomGameSelector.Models;
using RandomGameSelector.ViewModels;

namespace RandomGameSelector.Controllers
{
    public class GamesController : ControllerBase
    {
        public GamesController(RandomGameSelectorContext context) : base(context)
        {
        }


        /// <summary>
        /// Gets all genres from the DB
        /// </summary>
        /// <returns></returns>
        private List<Genre> GetGenres()
        {
            return _context.Genre.ToList();
        }
        
        private async Task<GameListPageModel> GetGamesList()
        {
            GameListPageModel listPage = new GameListPageModel();
            listPage.Games = await _context.Game.ToListAsync();
            listPage.Genres = await _context.Genre.ToListAsync();
            listPage.GameGenres = new List<string>();
            List<GameGenre> gameGenres = await _context.GameGenre.ToListAsync();

            //Going through each game so we can find which genres they are matched to.
            foreach (var game in listPage.Games)
            {
                //Grabbing all rows from pivot table using games Id
                List<int> genreIds = gameGenres.Where(x => x.GameId == game.Id).Select(x => x.GenreId).ToList();
                List<string> genreString = new List<string>();
                //Using the rows from the pivot table, grabbing all the matching genres and putting them into a string
                foreach (var genre in listPage.Genres)
                {
                    if (genreIds.Contains(genre.Id))
                    {
                        genreString.Add(genre.Name);
                    }
                }
                if (genreString.Count > 0)
                {
                    listPage.GameGenres.Add(string.Join(", ", genreString));
                }
                else
                {
                    listPage.GameGenres.Add("");
                }
            }
            
            return listPage;
        }

        /// <summary>
        /// Opens the Games List Page, has to get all the Genres and GameGenres for display.
        /// </summary>
        /// <returns></returns>
        // GET: Games
        public async Task<IActionResult> ListPage()
        {
            GameListPageModel listPage = await GetGamesList();

            return View("List", listPage);
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

            GameDetailModel gameDetail = new GameDetailModel();

            //Grabbing the genre data for a single job
            //Grabbing all rows from pivot table using games Id
            List<GameGenre> gameGenres = await _context.GameGenre.ToListAsync();
            List<int> genreIds = gameGenres.Where(x => x.GameId == game.Id).Select(x => x.GenreId).ToList();
            List<string> genreString = new List<string>();
            //Using the rows from the pivot table, grabbing all the matching genres and putting them into a string
            foreach (var genre in await _context.Genre.ToListAsync())
            {
                if (genreIds.Contains(genre.Id))
                {
                    genreString.Add(genre.Name);
                }
            }
            if (genreString.Count > 0)
            {
                gameDetail.Genres = string.Join(", ", genreString);
            }
            else
            {
                gameDetail.Genres = "";
            }

            gameDetail.Id = game.Id;
            gameDetail.Name = game.Name;

            return View(gameDetail);
        }

        /// <summary>
        /// Opens the edit game page with an empty item. This allows the page to dispaly as a create page.
        /// </summary>
        /// <returns></returns>
        // GET: Games/Edit
        public IActionResult Create()
        {
            GameEditViewModel gameGenres = new GameEditViewModel();
            gameGenres.AllGenres = GetGenres();
            return View("Edit", gameGenres);
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
        /// <summary>
        /// Creates and edits games.
        /// </summary>
        /// <param name="id">Game Id, if creating a Game then it is 0</param>
        /// <param name="name">Game Name</param>
        /// <param name="genres">Genre Ids that have been selected for this game</param>
        /// <returns></returns>
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

            //If the ModelState is valid, then we update the game.
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
            //If the ModelState is not valid, and we have an id of 0 and a non null name, we create
            //a new Game.
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGame(int id)
        {
             if (_context.Game == null)
             {
                 return Problem("Entity set 'RandomGameSelectorContext.Game'  is null.");
             }
             var game = await _context.Game.FindAsync(id);
             if (game != null)
             {
                RemoveGameGenreMappings(id, null);
                await _context.SaveChangesAsync();
                _context.Game.Remove(game);
             }
             
             await _context.SaveChangesAsync(); 
            return RedirectToAction(nameof(ListPage));
        }

        private bool GameExists(int id)
        {
          return _context.Game.Any(e => e.Id == id);
        }

        /// <summary>
        /// Adds GameGenre relationships to the GameGenre table.
        /// </summary>
        /// <param name="gameId">The Id of the New/Updated Game</param>
        /// <param name="gameGenreIds">The Ids of the Genres</param>
        private void UpdateGameGenres(int gameId, int[] gameGenreIds)
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

        public async Task<IActionResult> Index(string searchString, List<int>? selectedGenres)
        {
            GameListPageModel listPage = await GetGamesList();

            if (selectedGenres != null && selectedGenres.Count() > 0)
            {
                List<GameGenre> gameGenres = await _context.GameGenre.ToListAsync();
                List<Game> gamesToRemove = new List<Game>();
                gameGenres = gameGenres.Where(g => selectedGenres.Contains(g.GenreId)).ToList();
                foreach(Game game in listPage.Games)
                {
                    bool removeFlag = true;
                    foreach(GameGenre gameGenre in gameGenres)
                    {
                        if(gameGenre.GameId == game.Id)
                        {
                            removeFlag = false;
                        }
                    }

                    if (removeFlag)
                    {
                        gamesToRemove.Add(game);
                    }
                }

                if(gamesToRemove.Count > 0)
                {
                    foreach(Game game in gamesToRemove)
                    {
                        listPage.Games.Remove(game);
                    }
                }
            }

            //TODO need to fix the genres that are shown next to the games on the list since they are not updated.
            //TODO Need to have the filtered genres be preselected after filter is selected.

            if (!String.IsNullOrEmpty(searchString))
            {
                listPage.Games = (List<Game>?)listPage.Games.Where(g => g.Name.ToLower()!.Contains(searchString.ToLower())).ToList();
            }

            return View("List", listPage);
        }
    }
}
