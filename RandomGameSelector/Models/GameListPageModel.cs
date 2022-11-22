namespace RandomGameSelector.Models
{
    public class GameListPageModel
    {
        public List<Game>? Games { get; set; }

        public List<Genre>? Genres { get; set; }

        public List<string>? GameGenres { get; set; }
    }
}
