using RandomGameSelector.Models;

namespace RandomGameSelector.ViewModels
{
    public class GameEditViewModel
    {
        public Game? Game { get; set; }

        public List<Genre>? AllGenres { get; set; }

        public List<int>? MatchedGenreIds { get; set; }
    }
}
