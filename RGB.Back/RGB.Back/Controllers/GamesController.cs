﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public async Task<IEnumerable<GameDetailDTO>> GetGames()
		{
			var games = _service.GetAllGameDetail();
			return games;
		}

		[HttpPost("popular")]
		public async Task<IEnumerable<GameDetailDTO>> GetPopularGames(int begin, int end)
		{
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
		public async Task<IEnumerable<int>> GetCommendGames(int memberId)
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
					 .Select(g => g.GameId)
					 .ToListAsync();
				tagList.AddRange(tags);
			}

			//var gameIds = await _context.Collections
			//		 .GroupBy(x => x.GameId)
			//		 .OrderByDescending(g => g.Count())
			//		 .Select(g => g.Key)
			//		 .Take(6)
			//		 .ToListAsync();

			//var gameDTOs = new List<GameDetailDTO>();
			//foreach (var id in gameIds)
			//{
			//	var game = _service.GetGameDetailByGameId(id);
			//	gameDTOs.Add(game);
			//}

			//return gameDTOs;
			return tagList;
		}

		// GET: api/Games/5
		[HttpGet("{id}")]
		public async Task<GameDetailDTO> GetGame(int id)
		{
			var game = _service.GetGameDetailByGameId(id);
			return game;
		}

		// GET: api/Games/developer/5
		[HttpGet("developer/{developerId}")]
		public async Task<IEnumerable<GameDetailDTO>> GetGamesByDeveloperId(int developerId)
		{
			var games = _service.GetGameDetailByDeveloperId(developerId);
			return games;
		}

		// POST: api/Games/FilterByTags
		//public async Task<IEnumerable<GameDetailDTO>> FilterGamesByTags([FromBody] List<int> tagIds)
		[HttpPost("FilterByTags")]
		public async Task<IEnumerable<GameDetailDTO>> FilterGamesByTags(List<int> tagIds)
		{
			var games = _service.GetGameDetailByTags(tagIds);
			return games;
		}

		// GET: api/Games/developer/5
		[HttpGet("discount/{discountId}")]
		public async Task<IEnumerable<GameDetailDTO>> GetGamesByDiscount(int discountId)
		{
			var games = _service.GetDiscountedGames(discountId);
			return games;
		}

		[HttpGet("dlc/{dlcId}")]
		public async Task<GameDetailDTO> GetMainGame(int dlcId)
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
		public Task<string> AddToWishList(WishListe wishListe)
		{
			try
			{
			_context.WishListes.Add(wishListe);
			_context.SaveChanges();
			return Task.FromResult("Success");

			}catch(Exception e)
			{
				return Task.FromResult(e.Message);
			}
		}
	}
}