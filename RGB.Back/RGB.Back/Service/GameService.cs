using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
	public class GameService
	{
		private readonly RizzContext _context;
		public GameService(RizzContext context)
        {
			_context = context;
		}
        public GameDetailDTO GetGameDetailByGameId(int gameId)
		{			
			return new GameDetailDTO();
		}

		public GameDetailDTO GetGameDetailByDeveloperId(int developerId)
		{
			return new GameDetailDTO();
		}

		public List<Tag> GetTags(int? gameId)
		{
			return new List<Tag>();
		}

		public List<Game> GetDLCs(int gameId)
		{
			return new List<Game>();
		}

		public List<string> GetImages(int gameId)
		{
			return new List<string>();
		}

		public List<DiscountItem> GetDiscounts(int? gameId)
		{
			return new List<DiscountItem>();
		}
	}


}
