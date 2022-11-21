namespace RandomGameSelector.Models
{
    public class GameGenres
    {
        public Game? Game { get; set; }

        public List<Genre>? AllGenres { get; set; }

        public List<int>? MatchedGenreIds { get; set; }
    }
}
