using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Model.Index;
using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using RGB.Back.Service;

namespace RGB.Back.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class GamesController : ControllerBase
	{
		private readonly RizzContext _context;
		private readonly GameService _service;

		public GamesController(RizzContext context)
		{
			_context = context;
			_service = new GameService(context);
		}

		// GET: api/Games
		[HttpGet]
		public IEnumerable<GameDetailDTO> GetGames()
		{
			var games = _service.GetAllGameDetail();
			return games;
		}

        [HttpGet("new")]
        public async Task<IEnumerable<GameDetailDTO>> GetNewGames()
        {
			var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
            var gameIds = await _context.Games
					 .Where(g => g.ReleaseDate <= currentDate)
                     .OrderByDescending(g => g.ReleaseDate)
                     .Select(g => g.Id)
                     .Take(6)
                     .ToListAsync();

            var gameDTOs = new List<GameDetailDTO>();
            foreach (var id in gameIds)
            {
                var game = _service.GetGameDetailByGameId(id);
                gameDTOs.Add(game);
            }

            return gameDTOs;
        }

        [HttpPost("popular")]
		public async Task<IEnumerable<GameDetailDTO>> GetPopularGames(int begin, int end)
		{
			//var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
			var gameIds = await _context.Collections
					 .GroupBy(x => x.GameId) 
					 .OrderByDescending(g => g.Count()) 
					 .Select(g => g.Key) 
					 .Skip(begin)
					 .Take(end)
					 .ToListAsync();

			var gameDTOs = new List<GameDetailDTO>();
			foreach (var id in gameIds)
			{
				var game = _service.GetGameDetailByGameId(id);
				gameDTOs.Add(game);
			}

			return gameDTOs;
		}

		[HttpPost("Commend")]
		public async Task<IEnumerable<GameDetailDTO>> GetCommendGames(int memberId)
		{
			var gameIds = await _context.Collections
					 .Where(x=> x.MemberId == memberId)
					 .Select(x => x.GameId)
					 .ToListAsync();

			var tagList = new List<int>();
			foreach (var id in gameIds)
			{
				var tags = await _context.GameTags
					 .Where(x => x.GameId == id)
					 .Select(g => g.TagId)
					 .ToListAsync();
				tagList.AddRange(tags);
			}

			var sortedTag = tagList
			.GroupBy(x => x)
			.OrderByDescending(g => g.Count())
			.Select(g => g.Key)
			.ToList();

			var max = 6;
			var i = 0;
			var notEnughtNum = 6;
			var commendGameList = new List<int>();

			while (commendGameList.Count < max)
			{
				notEnughtNum = max - commendGameList.Count();

				var commendGame = _context.GameTags.AsNoTracking()
				.Where(x => x.TagId == sortedTag[i])
				.Select(x => x.GameId)
				.ToList();

				var filteredCommendGame = commendGame.Except(gameIds).Except(commendGameList).ToList();

				commendGameList.AddRange(filteredCommendGame.Take(notEnughtNum));

				i++;
			}

			var gameDTOs = new List<GameDetailDTO>();
			foreach (var id in commendGameList)
			{
				gameDTOs.Add(_service.GetGameDetailByGameId(id));
			}

			return gameDTOs;

		}

		// GET: api/Games/5
		[HttpGet("{id}")]
		public GameDetailDTO GetGame(int id)
		{
			var game = _service.GetGameDetailByGameId(id);
			return game;
		}

		// GET: api/Games/developer/5
		[HttpGet("developer/{developerId}")]
		public IEnumerable<GameDetailDTO> GetGamesByDeveloperId(int developerId)
		{
			var games = _service.GetGameDetailByDeveloperId(developerId);
			return games;
		}

		// POST: api/Games/FilterByTags
		//public async Task<IEnumerable<GameDetailDTO>> FilterGamesByTags([FromBody] List<int> tagIds)
		[HttpPost("FilterByTags")]
		public IEnumerable<GameDetailDTO> FilterGamesByTags(List<int> tagIds)
		{
			var games = _service.GetGameDetailByTags(tagIds);
			return games;
		}

		// GET: api/Games/developer/5
		[HttpGet("discount/{discountId}")]
		public IEnumerable<GameDetailDTO> GetGamesByDiscount(int discountId)
		{
			var games = _service.GetDiscountedGames(discountId);
			return games;
		}

        [HttpGet("alldiscount")]
        public IEnumerable<GameDetailDTO> GetAllDiscountGames()
        {
			var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);
			var discountList = _context.Discounts.Where(x => x.StartDate <= currentDate && x.EndDate >= currentDate);
			var gameList = new List<GameDetailDTO>();
			foreach (var discount in discountList)
			{
				gameList.AddRange(_service.GetDiscountedGames(discount.Id));
                
			}
            return gameList;
        }

        [HttpGet("dlc/{dlcId}")]
		public GameDetailDTO GetMainGame(int dlcId)
		{

			var games = _service.GetMainGame(dlcId);
			return games;
		}

		[HttpGet("developerName/{developerId}")]
		public string GetDeveloperName(int developerId)
		{
			return _context.Developers.AsNoTracking()
				.Where(x => x.Id == developerId)
				.Select(x => x.Name)
				.FirstOrDefault();
		}

		[HttpGet("tag/{tagId}")]
		public string GetTagName(int tagId)
		{
			var tag = _context.Tags.AsNoTracking()
				.Where(x => x.Id == tagId)
				.Select(x => x.Name)
				.FirstOrDefault();

			return tag;
		}

		[HttpPost("AddToWishList")]
		public async Task AddToWishList(WishListe wishListe)
		{
			var boolIsHave = _context.WishListes.Any(x => x.MemberId == wishListe.MemberId && x.GameId == wishListe.GameId);
			if(!boolIsHave)
			{
			_context.WishListes.Add(wishListe);
				_context.SaveChanges();
			}	
				
		}

		public class gameData
		{
            public int gameId { get; set; }
            public int memberId { get; set; }
        }

		[HttpPost("IsHaveGame")]
		public bool IsHaveGame(gameData _gameData)
        {
			return _context.Collections.AsNoTracking().Where(x=> x.MemberId == _gameData.memberId && x.GameId == _gameData.gameId).Any();
		}

		[HttpGet("GetWishList")]
		public List<GameDetailDTO> GetWishList(int memberId) 
		{
			var gameIdList = _context.WishListes.Where(x=>x.MemberId == memberId).Select(x=> x.GameId).ToList();

			var gameList = new List<GameDetailDTO>();
			foreach (var gameId in gameIdList) 
			{
				gameList.Add(_service.GetGameDetailByGameId(gameId));
            }
			return gameList;
		}

		[HttpDelete("DeleteWishList")]
		public void DeleteWishList(int memberId, int gameId) 
		{
			var wishList = _context.WishListes.AsNoTracking().Where(x => x.MemberId == memberId && x.GameId == gameId).FirstOrDefault();
            _context.WishListes.Remove(wishList);
            _context.SaveChanges();
        }
    }
}