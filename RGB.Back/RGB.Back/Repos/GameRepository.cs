using RGB.Back.Models.ViewModels;

namespace RGB.Back.Repo
{
    public class GameRepository
    {
        private static List<Game> _games;
        static GameRepository()
        {
            _games = Enumerable.Range(1, 10).Select(i => new Game
            {
                Id = i,
                Name = $"Game {i}",
                Price = i * 1000
            }).ToList();
        }
        public Game Get(int id)
        {
            return _games.FirstOrDefault(p => p.Id == id);
        }
    }
}
