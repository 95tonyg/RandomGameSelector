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

    public class GenresController : ControllerBase
    {
        public GenresController(RandomGameSelectorContext context) : base(context)
        {
        }

        /// <summary>
        /// Opens the Genres List Page
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> ListPage()
        {

            return View("List", await _context.Genre.ToListAsync());
        }

        //GET: Games/CreateGenre
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Create([Bind("Id,Name")] Genre genre)
        {
            if (ModelState.IsValid)
            {
                _context.Add(genre);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ListPage));
            }
            return View(genre);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Genre == null)
            {
                return NotFound();
            }

            var genre = await _context.Genre.FindAsync(id);
            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }

        [HttpPost, ActionName("DeleteGenre")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteGenre(int? id)
        {
            if (_context.Genre == null)
            {
                return Problem("Entity set 'RandomGameSelectorContext.Genre'  is null.");
            }
            var genre = await _context.Genre.FindAsync(id);
            if (genre != null)
            {
                RemoveGameGenreMappings(null, id);
                await _context.SaveChangesAsync();
                _context.Genre.Remove(genre);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ListPage));
        }

        public async Task<IActionResult> Index(string searchString)
        {
            var genres = await _context.Genre.ToListAsync();

            if (!String.IsNullOrEmpty(searchString))
            {
                genres = genres.Where(g => g.Name.ToLower()!.Contains(searchString.ToLower())).ToList();
            }

            ViewData["SearchString"] = searchString;

            return View("List", genres);
        }
    }
}
