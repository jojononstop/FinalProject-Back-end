using RGB.Back.DTOs;
using RGB.Back.Models;
using Microsoft.EntityFrameworkCore;

namespace RGB.Back.Service
{
	public class CollectionService
	{
		private readonly RizzContext _context;
		public CollectionService(RizzContext context)
		{
			_context = context;
		}

		public List<CollectionDTO> GetCollectionDetailByMemberId(int memberId)
		{

			var collectionList = _context.Collections.AsNoTracking()
				.Where(c => c.MemberId == memberId)
				.Include(c=>c.BillDetail)
				.Select(c => new CollectionDTO
				{
					Id = c.Id,
					GameId = c.GameId,
					MemberTagId =c.MemberTagId,
					DateOfPurchase = c.BillDetail.TransactionDate
				})
				.Distinct()
				.ToList();

			return collectionList;
		}

		public void UpdateCollectionTags(CollectionDTO dto)
		{
			Collection model = _context.Collections.Find(dto.Id);

			model.MemberTagId = dto.MemberTagId;

			_context.SaveChanges();
		}
	}
}
