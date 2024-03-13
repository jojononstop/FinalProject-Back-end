using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using RGB.Back.DTOs;
using RGB.Back.Infra;
using RGB.Back.Models;
using System;

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
		public List<MemberTagDTO> GetMemberTagByMemberId(int memberId)
		{
			var memberTagList = _context.MemberTags.AsNoTracking()
				.Where(mt => mt.MemberId == memberId)
				.Select(mt => new MemberTagDTO
				{
					Id = mt.Id,
					Name = mt.Name,
				})
				.Distinct()
				.ToList();

			return memberTagList;
		}

		public bool ValidLogin(LoginDTO loginDto)
		{

			// 根據account(帳號)取得 Member
			var member = _context.Members.FirstOrDefault(p => p.Account == loginDto.Account);
			if (member == null)
			{
				return false;// 原則上, 不要告知細節
			}

			//// 檢查是否已經確認
			//if (member.IsConfirmed == false)
			//{
			//	throw new Exception("您尚未開通會員資格, 請先收確認信, 並點選信裡的連結, 完成認證, 才能登入本網站");
			//}

			//// 將vm裡的密碼先雜湊之後,再與db裡的密碼比對
			//var salt = HashUtility.GetSalt();
			//var hashedPassword = HashUtility.ToSHA256(loginDto.Password, salt);

			if (string.Compare(member.Password, loginDto.Password, true) != 0)
			{
				return false;
			}
			return true;
		}
	}
}
