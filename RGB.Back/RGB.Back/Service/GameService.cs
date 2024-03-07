using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;
using System;

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
			var gameDto = new GameDetailDTO();

			var game = _context.Games.AsNoTracking()
				.Where(x => x.Id == gameId)
				.FirstOrDefault();

			var tags = GetTags(gameId);
			var discounts = GetDiscounts(gameId);
			var DLCs = GetDLCs(gameId);
			var images = GetImages(gameId);

			gameDto.Id = game.Id;
			gameDto.Name = game.Name;
			gameDto.Introduction = game.Introduction;
			gameDto.Price = game.Price;
			gameDto.ReleaseDate = new DateTime(game.ReleaseDate.Year, game.ReleaseDate.Month, game.ReleaseDate.Day);
			gameDto.Cover = game.Cover;
			gameDto.MaxPercent = game.MaxPercent;
			gameDto.Description = game.Description;
			gameDto.DeveloperId = game.DeveloperId;
			gameDto.Video = game.Video;
			gameDto.Tags = tags;
			gameDto.Discounts = discounts;
			gameDto.DLCs = DLCs;
			gameDto.DisplayImages = images;
			
			return gameDto;
		}

		public GameDetailDTO GetGameDetailByDeveloperId(int developerId)
		{
			return new GameDetailDTO();
		}

		public List<Tag> GetTags(int? gameId)
		{
			if (gameId != null)
			{				
				return _context.GameTags.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.Join(_context.Tags, 
					x => x.TagId,
					y => y.Id,
					(x, y) => y) 
					.ToList();
			}
			else
			{
				return _context.Tags.ToList();
			}
		}

		public List<Game> GetDLCs(int gameId)
		{
			return _context.Dlcs.AsNoTracking()
				.Where(x => x.MainGameId == gameId)
				.Join(_context.Games,
				x => x.DlcId,
				y => y.Id,
				(x, y) => y)
				.ToList();
		}

		public List<string> GetImages(int gameId)
		{
			return _context.Images.AsNoTracking()
				.Where(x=> x.GameId == gameId)
				.Select(x => x.DisplayImage)
				.ToList();
		}

		public List<Discount> GetDiscounts(int? gameId)
		{
			var today = DateOnly.FromDateTime(DateTime.Now);
			if (gameId != null)
			{
				return _context.DiscountItems.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.Join(_context.Discounts,
					x => x.DiscountId,
					y => y.Id,
					(x, y) => y)
					.Where(x => x.StartDate <= today && x.EndDate >= today)
					.ToList();
			}
			else
			{
				return _context.Discounts.AsNoTracking()					
					.Where(x => x.StartDate <= today && x.EndDate >= today)
					.ToList();
			}
				
		}

		public List<Comment> GetComments(int gameId)
		{
			return _context.Comments.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.OrderByDescending(x => x.Id)
					.ToList();
		}

		public double GetRating(int gameId)
		{
			var comments = _context.Comments.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.OrderByDescending(x => x.Id)
					.ToList();

			var rating = 0;

			foreach (var comment in comments) 
			{ rating += comment.Rating; }

			var avrageRating = (double)rating / comments.Count;

			return avrageRating;
		}
	}


}
