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
    public class ControllerBase : Controller
    {
        protected readonly RandomGameSelectorContext _context;

        public ControllerBase(RandomGameSelectorContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Passed in either a gameId or genreId. This function is called when a game is being deleted.
        /// since the GameGenre table has foreign keys, we need to delete any records that contain a key 
        /// that is being deleted.
        /// </summary>
        /// <param name="gameId">If a game has been deleted. A gameId is passed in</param>
        /// <param name="genreId">If a genre has been deleted. A genreId is passed in</param>
        protected void RemoveGameGenreMappings(int? gameId, int? genreId)
        {
            if (gameId != null)
            {
                _context.GameGenre.RemoveRange(_context.GameGenre.Where(x => x.GameId == gameId));
            }
            else
            {
                _context.GameGenre.RemoveRange(_context.GameGenre.Where(x => x.GenreId == genreId));
            }
        }
    }
}
