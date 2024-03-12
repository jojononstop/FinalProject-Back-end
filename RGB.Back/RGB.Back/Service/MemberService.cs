using Microsoft.EntityFrameworkCore;
using RGB.Back.DTOs;
using RGB.Back.Models;

namespace RGB.Back.Service
{
	public class MemberService
	{
		private readonly RizzContext _context;
		public MemberService(RizzContext context)
		{
			_context = context;
		}

		public MemberDTO GetMemberDetailByMemberId(int memberId)
		{
			var memberDto = new MemberDTO();
			var member = _context.Members.AsNoTracking()
	                  .Where(x => x.Id == memberId)
	                  .FirstOrDefault();
			memberDto.Id = member.Id;
			memberDto.Account = member.Account;
			memberDto.Mail = member.Mail;
			memberDto.AvatarUrl = member.AvatarUrl;
			memberDto.RegistrationDate = member.RegistrationDate;
			memberDto.BanTime = member.BanTime;
			memberDto.Bonus = member.Bonus;
			memberDto.LastLoginDate = member.LastLoginDate;
			memberDto.Birthday = member.Birthday;
			memberDto.NickName = member.NickName;

			return memberDto;
		}


	}
}
