using RandomGameSelector.Models;

namespace RandomGameSelector.ViewModels
{
    public class GameListPageModel
    {
        public List<Game>? Games { get; set; }

        public List<Genre>? Genres { get; set; }

        public Dictionary<int, string>? GameGenres { get; set; }

        public List<int>? SelectedGenres { get; set; }
    }
}
