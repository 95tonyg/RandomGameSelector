using RandomGameSelector.Models;

namespace RandomGameSelector.ViewModels
{
    public class GameDetailModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Genres { get; set; }
    }
}
