using System;
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
			return _service.GetAllGameDetail();
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
		[HttpPost("FilterByTags")]
		public async Task<IEnumerable<GameDetailDTO>> FilterGamesByTags([FromBody] List<int> tagIds)
		{
			var games = _service.GetGameDetailByTags(tagIds);
			return games;
		}

		// GET: api/Games/developer/5
		[HttpGet("discount/{discountId}")]
		public async Task<IEnumerable<GameDetailDTO>> GetGamesByDiscount(int dlcId)
		{
			var games = _service.GetDiscountedGames(dlcId);
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

		// PUT: api/Games/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPut("{id}")]
		//public async Task<IActionResult> PutGame(int id, Game game)
		//{
		//    if (id != game.Id)
		//    {
		//        return BadRequest();
		//    }

		//    _context.Entry(game).State = EntityState.Modified;

		//    try
		//    {
		//        await _context.SaveChangesAsync();
		//    }
		//    catch (DbUpdateConcurrencyException)
		//    {
		//        if (!GameExists(id))
		//        {
		//            return NotFound();
		//        }
		//        else
		//        {
		//            throw;
		//        }
		//    }

		//    return NoContent();
		//}

		//// POST: api/Games
		//// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//[HttpPost]
		//public async Task<ActionResult<Game>> PostGame(Game game)
		//{
		//    _context.Games.Add(game);
		//    await _context.SaveChangesAsync();

		//    return CreatedAtAction("GetGame", new { id = game.Id }, game);
		//}

		//// DELETE: api/Games/5
		//[HttpDelete("{id}")]
		//public async Task<IActionResult> DeleteGame(int id)
		//{
		//    var game = await _context.Games.FindAsync(id);
		//    if (game == null)
		//    {
		//        return NotFound();
		//    }

		//    _context.Games.Remove(game);
		//    await _context.SaveChangesAsync();

		//    return NoContent();
		//}

		//private bool GameExists(int id)
		//{
		//    return _context.Games.Any(e => e.Id == id);
		//}
	}
}
