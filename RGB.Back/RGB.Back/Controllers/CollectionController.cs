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
	public class CollectionController
	{
		private readonly RizzContext _context;
		private readonly CollectionService _service;

		public CollectionController(RizzContext context)
		{
			_context = context;
			_service = new CollectionService(context);
		}

		[HttpGet]
		public async Task<IEnumerable<CollectionDTO>> GetGames(int memberId)
		{
			return _service.GetCollectionDetailByMemberId(memberId);
		}
	}


}
