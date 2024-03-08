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

		public List<GameDetailDTO> GetAllGameDetail()
		{
			var games = _context.Games.AsNoTracking().ToList();

			return GameToGameDetailDTO(games);
		}

		public List<GameDetailDTO> GetGameDetailByTags(List<int> tagIds)
		{
			var games = _context.Games.AsNoTracking().ToList();

			foreach (int tagId in tagIds)
			{
				games = games.Join(_context.GameTags.Where(x => x.TagId == tagId),
								   g => g.Id,
								   gt => gt.GameId,
								   (g, gt) => g)
							   .ToList();
			}

			return GameToGameDetailDTO(games);
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
			var ratings = GetRating(gameId);

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
			gameDto.Rating = ratings;

			return gameDto;
		}

		public List<GameDetailDTO> GetGameDetailByDeveloperId(int developerId)
		{
			var games = _context.Games.AsNoTracking()
				.Where(x => x.DeveloperId == developerId)
				.ToList();
	
			return GameToGameDetailDTO(games);
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

		public List<DlcDetailDTO> GetDLCs(int gameId)
		{
			var dlcList = new List<DlcDetailDTO>();
			var dlcs = _context.Dlcs.AsNoTracking()
				.Where(x => x.MainGameId == gameId)
				.Join(_context.Games,
				x => x.DlcId,
				y => y.Id,
				(x, y) => y)
				.ToList();

			foreach (var dlc in dlcs)
			{
				var dlcDto = new DlcDetailDTO();

				var tags = GetTags(dlc.Id);
				var discounts = GetDiscounts(dlc.Id);
				var images = GetImages(dlc.Id);
				var ratings = GetRating(dlc.Id);

				dlcDto.Id = dlc.Id;
				dlcDto.Name = dlc.Name;
				dlcDto.Introduction = dlc.Introduction;
				dlcDto.Price = dlc.Price;
				dlcDto.ReleaseDate = new DateTime(dlc.ReleaseDate.Year, dlc.ReleaseDate.Month, dlc.ReleaseDate.Day);
				dlcDto.Cover = dlc.Cover;
				dlcDto.MaxPercent = dlc.MaxPercent;
				dlcDto.Description = dlc.Description;
				dlcDto.DeveloperId = dlc.DeveloperId;
				dlcDto.Video = dlc.Video;
				dlcDto.Tags = tags;
				dlcDto.Discounts = discounts;
				dlcDto.DisplayImages = images;
				dlcDto.Rating = ratings;

				dlcList.Add(dlcDto);
			}

			return dlcList;
		}

		public List<string> GetImages(int gameId)
		{
			return _context.Images.AsNoTracking()
				.Where(x => x.GameId == gameId)
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

		public List<CommentDTO> GetComments(int gameId)
		{
			var comments = _context.Comments.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.Select(x => new CommentDTO
					{
						Id = x.Id,
						MemberId = x.MemberId,
						GameId = x.GameId,
						Rating = x.Rating,
						Comment1 = x.Comment1,
						Date = x.Date
					})
					.OrderByDescending(x => x.Id)
					.ToList();

			foreach (var comment in comments)
			{
				var attachedComments = _context.AttachedComments.AsNoTracking()
					.Where(x => x.MainCommentId == comment.Id)
					.ToList();

				comment.AttachedComment = attachedComments;
			}

			return comments;
		}

		public double GetRating(int gameId)
		{
			var comments = _context.Comments.AsNoTracking()
					.Where(x => x.GameId == gameId)
					.OrderByDescending(x => x.Id)
					.ToList();

			var rating = 0;

			if (comments.Count == 0) { 
				return 0; 
			}else{
				foreach (var comment in comments)
				{ rating += comment.Rating; }

				return (double)rating / comments.Count;
			}
		}

		public List<GameDetailDTO> GameToGameDetailDTO(List<Game> games)
		{
			var gameList = new List<GameDetailDTO>();

			foreach (var game in games)
			{
				var gameDto = new GameDetailDTO();
				var tags = GetTags(game.Id);
				var discounts = GetDiscounts(game.Id);
				var DLCs = GetDLCs(game.Id);
				var images = GetImages(game.Id);
				var ratings = GetRating(game.Id);

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
				gameDto.Rating = ratings;

				gameList.Add(gameDto);
			}

			return gameList;
		}
	}


}
