using RGB.Back.DTOs;
using RGB.Back.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace RGB.Back.Service
{
	public class CollectionService
	{
		private readonly RizzContext _context;
		public CollectionService(RizzContext context)
		{
			_context = context;
		}

		public List<int> GetCollectionDetailByMemberId(int memberId)
		{

			var collectiongameIdList = _context.Collections.AsNoTracking()
				.Where(c => c.MemberId == memberId)
				.Select(c=>c.GameId)
				.Distinct()
				.ToList();

			return collectiongameIdList;
		}

		public List<CollectionGameDTO> GameIdFindGameDetail(List<int> gameIds) 
		{

			var collectiongameList = new List<CollectionGameDTO>();
			foreach (int gameId in gameIds)
			{
				List<int> dlcIds = _context.Dlcs.AsNoTracking()
						.Where(d=>d.MainGameId == gameId)
						.Select(d=>d.DlcId)
						.ToList();
				List<DlcCollectionDTO> dlcList = new List<DlcCollectionDTO>();
				if (dlcIds != null)
				{
					foreach (var dlcId in dlcIds)
					{
						var dlc = _context.Games.AsNoTracking()
							  .Where(g => g.Id == dlcId)
							  .Select(g =>new DlcCollectionDTO
							  {
								  Id = g.Id,
								  Name = g.Name,
							  })
							  .FirstOrDefault();
						if (dlc != null)
						{
							dlcList.Add(dlc);
						}
					}
				}


				var game = _context.Games.AsNoTracking()
					.Where(g => g.Id == gameId)
					//.Select(g => g.Id,DeveloperId = g.DeveloperId);
					.Select(g => new CollectionGameDTO
					{
						Id = g.Id,
						DeveloperId = g.DeveloperId,
						Name = g.Name,
						Cover = g.Cover,
						Dlcs = dlcList
					})
					.FirstOrDefault(); ;

				var isdlc = _context.Dlcs.AsNoTracking()
					.Where(d => d.DlcId == gameId)
					.FirstOrDefault();
				if (isdlc == null)
				{
					collectiongameList.Add(game);
				}
				
			}
			return collectiongameList;
		}



		//public List<CollectionDTO> GetCollectionDetailByMemberId(int memberId)
		//{

		//	var collectionList = _context.Collections.AsNoTracking()
		//		.Where(c => c.MemberId == memberId)

		//		.Select(c => new CollectionDTO
		//		{
		//			Id = c.Id,
		//			GameId = c.GameId,
		//			MemberTagId = c.MemberTagId,
		//		})
		//		.Distinct()
		//		.ToList();

		//	return collectionList;
		//}


		public void UpdateCollectionTags(int Id,int memberTagId)
		{
			Collection model = _context.Collections.Find(Id);

			model.MemberTagId = memberTagId;

			_context.SaveChanges();
		}




		//轉dto
		
	}
}
