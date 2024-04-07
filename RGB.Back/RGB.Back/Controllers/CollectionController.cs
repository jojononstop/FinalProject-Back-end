using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.DataProtection;
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
		private readonly IDataProtector _dataProtector;

		public CollectionController(RizzContext context)
		{
			_context = context;
			_service = new CollectionService(context);

			// 创建服务集合
			var serviceCollection = new ServiceCollection();
			// 向服务集合中添加数据保护服务
			serviceCollection.AddDataProtection();
			// 构建服务提供程序
			var serviceProvider = serviceCollection.BuildServiceProvider();
			// 从服务提供程序中获取数据保护服务
			var dataProtectionProvider = serviceProvider.GetRequiredService<IDataProtectionProvider>();
			// 建立保護器 
			_dataProtector = dataProtectionProvider.CreateProtector("SamplePurpose");
		}

		//<IEnumerable<CollectionDTO>>
		//搜索所有收藏
		[HttpGet("collectiongames")]
		public async Task<List<CollectionGameDTO>> GetGames(string memberId)
		{
			var unprotectId = _dataProtector.Unprotect(memberId);
			var collectiongameidList = _service.GetCollectionDetailByMemberId(Convert.ToInt32(unprotectId));
			var collectiongameList = _service.GameIdFindGameDetail(collectiongameidList);
			return collectiongameList;
		}
		
		//把收藏的遊戲資料找出來
	}


}
