using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using RGB.Back.DTOs;
using RGB.Back.Infra;
using RGB.Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
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

		public (bool, int) ValidLogin(LoginDTO loginDto)
		{

			// 根據account(帳號)取得 Member
			var member = _context.Members.FirstOrDefault(p => p.Account == loginDto.Account);
			if (member == null)
			{
				return (false,0);
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
				return (false, 0);
			}
			else 
			{
				member.LastLoginDate=DateTime.Now;
				_context.SaveChanges();
				return (true, member.Id);
			}
		}

		public (bool, int) FindMemberIdByGoogle(string GoogleMail)
		{
			var member = _context.Members.FirstOrDefault(p => p.Google == GoogleMail);
			if (member == null)
			{
				return (false, 0);
			}
			else { 
				return (true, member.Id); 
			}
		}
		//work
		// 創建會員
		public string CreateMember(CreateMemberDTO cmDto)
		{
			var MemberInDb = _context.Members.FirstOrDefault(p => p.Account == cmDto.Account);
			if (MemberInDb != null)
			{
				return "帳號已經存在";
			}
			var confirmCode = Guid.NewGuid().ToString("N");
			Member member = new Member()
			{
				Account = cmDto.Account,
				//待加密
				Password = cmDto.Password,
				Mail = cmDto.Mail,
				AvatarUrl = null,
				RegistrationDate = DateTime.Now,
				BanTime = null,
				Bonus = 0,
				LastLoginDate = DateTime.Now,
				Birthday = null,
				IsConfirmed = false,
				ConfirmCode = confirmCode,
				Google = null,
				Role = null
			};
			_context.Members.Add(member);
			_context.SaveChanges();

			// 發出確認信

			//var urlTemplate = Request.Url.Scheme + "://" +  // 生成 http:.// 或 https://
			//				Request.Url.Authority + "/" + // 生成網域名稱或 ip
			//				"Developer/ActiveRegister?developerid={0}&confirmCode={1}";
			//var url = string.Format(urlTemplate, developer.Id, developer.ConfirmCode);
			//string name = vm.Name;
			//string email = vm.EMail;
			//前台網站
			var url = "";
			string name = cmDto.Account; // 請確認您的 CreateMemberDTO 類中是否包含了名稱（Name）和電子郵件（EMail）屬性
			string email = cmDto.Mail;
			new EMailHelper().SendConfirmRegisterEmail(url, name, email);

			return "註冊完成";
		}

		//發送驗證信
		public void SendConfirmationEmail() 
		{

		}

		public bool ActiveRegister (int memberId, string confirmCode)
		{
			//驗證傳入值是否合理

			if (memberId <= 0 || string.IsNullOrEmpty(confirmCode))
			{
				return false; // 在view中,我們會顯示'已開通,謝謝'
			}

			// 根據 Id, confirmCode 取得 未確認的 member
			Member member = _context.Members.FirstOrDefault(m => m.Id == memberId && m.IsConfirmed == false && m.ConfirmCode == confirmCode);
			if (member == null) return false;

			// 如果有找到, 將它更新為已確認
			member.IsConfirmed = true; // 視為已確認的會員
			member.ConfirmCode = null; // 清空 confirm code 欄位

			_context.SaveChanges();

			return true;
		}
	}
}
