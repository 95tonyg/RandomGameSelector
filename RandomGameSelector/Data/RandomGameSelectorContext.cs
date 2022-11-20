using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RandomGameSelector.Models;

namespace RandomGameSelector.Data
{
    public class RandomGameSelectorContext : DbContext
    {
        public RandomGameSelectorContext (DbContextOptions<RandomGameSelectorContext> options)
            : base(options)
        {
        }

        public DbSet<RandomGameSelector.Models.Game> Game { get; set; } = default!;
        public DbSet<RandomGameSelector.Models.Genre> Genre { get; set; } = default!;
    }
}
