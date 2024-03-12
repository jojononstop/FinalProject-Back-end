using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
	public class MemberTagService
	{
		private readonly RizzContext _context;
		public MemberTagService(RizzContext context)
		{
			_context = context;
		}

		public void Create(MemberTagDTO dto)
		{
			var memberTag = new MemberTag           
			{
				Id = dto.Id,
				Name = dto.Name,
			};

			_context.MemberTags.Add(memberTag);
			_context.SaveChanges();
		}

		public void Update(MemberTagDTO dto)
		{
			MemberTag model = _context.MemberTags.Find(dto.Id);

			model.Name=dto.Name;

			_context.SaveChanges();
		}
	}

}
