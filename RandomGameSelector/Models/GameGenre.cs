using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RandomGameSelector.Models
{
    [PrimaryKey(nameof(GameId), nameof(GenreId))]
    public class GameGenre
    {
        public int GameId { get; set; }

        public int GenreId { get; set; }

        public GameGenre(int gameId, int genreId)
        {
            GameId = gameId;
            GenreId = genreId;
        }
    }
}
