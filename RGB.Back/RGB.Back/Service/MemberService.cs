using Azure.Core;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using RGB.Back.DTOs;
using RGB.Back.Infra;
using RGB.Back.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Security.Cryptography;
using System.Text;
using static System.Net.WebRequestMethods;

namespace RGB.Back.Service
{
	public class MemberService
	{
		private readonly RizzContext _context;
		private readonly RSAEncryptor _rsaService;
		public MemberService(RizzContext context)
		{
			_context = context;
			_rsaService = new RSAEncryptor("private_key.xml", "public_key.xml");
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

		public (bool,int, MemberDataDTO) ValidLogin(LoginDTO loginDto)
		{

			// 根據account(帳號)取得 Member
			var member = _context.Members.FirstOrDefault(p => p.Account == loginDto.Account);
			if (member == null)
			{
				var dto = new MemberDataDTO
				{
					Id =0,
					AvatarUrl=null,
					Bonus=0,
					NickName=""
				};
				return (false,0, dto);
			}

			//解密
			var unencryptedpassword = _rsaService.Decrypt(member.Password);
			if (string.Compare(unencryptedpassword, loginDto.Password, true) != 0)
			{
				var dto = new MemberDataDTO
				{
					Id = 0,
					AvatarUrl = null,
					Bonus = 0,
					NickName = ""
				};
				return (false,0, dto);
			}
			// 檢查是否已經確認
			if (member.IsConfirmed == false)
			{
				var dto = new MemberDataDTO
				{
					Id = 0,
					AvatarUrl = null,
					Bonus = 0,
					NickName = ""
				};
				return (false, 1, dto);
				//throw new Exception("您尚未開通會員資格, 請先收確認信, 並點選信裡的連結, 完成認證, 才能登入本網站");
			}
			else 
			{
				member.LastLoginDate=DateTime.Now;
				_context.SaveChanges();
				
				var Avatar = "";
				if (member.AvatarUrl==null)
				{
					//預設頭像
					Avatar = "https://bootdey.com/img/Content/avatar/avatar2.png";
				}
				else
				{
					Avatar = member.AvatarUrl;
				}
				var dto = new MemberDataDTO
				{
					Id = member.Id,
					AvatarUrl = Avatar,
					Bonus = member.Bonus,
					NickName = member.NickName
				};
				return (true,2, dto);
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
			//加密
			var encryptedpassword = _rsaService.Encrypt(cmDto.Password);
			var confirmCode = Guid.NewGuid().ToString("N");
			Member member = new Member()
			{
				Account = cmDto.Account,
				//待加密
				Password = encryptedpassword,
				Mail = cmDto.Email,
				//預設頭像
				AvatarUrl = "https://bootdey.com/img/Content/avatar/avatar2.png",
				RegistrationDate = DateTime.Now,
				BanTime = null,
				Bonus = 0,
				LastLoginDate = DateTime.Now,
				Birthday = null,
				NickName = cmDto.Name,
				IsConfirmed = false,
				ConfirmCode = confirmCode,
				Google = cmDto.Google,
				Role = null
			};
			_context.Members.Add(member);
			_context.SaveChanges();

			// 發出確認信
			//http://localhost:3000/Id=1/ActiveconfirmCode=jk
			var urlTemplate = "http" + "://" +  // 生成 http:.// 或 https://
							"localhost:3000" + "/" + // 生成網域名稱或 ip
							"Id={0}" + "/" +
							"ActiveconfirmCode={1}";
			var url = string.Format(urlTemplate, member.Id, member.ConfirmCode);
			string name = cmDto.Account; // 請確認您的 CreateMemberDTO 類中是否包含了名稱（Name）和電子郵件（EMail）屬性
			string email = cmDto.Email;
			new EMailHelper().SendConfirmRegisterEmail(url, name, email);
			//前台網站
			//var url = "";
			//string name = cmDto.Account; // 請確認您的 CreateMemberDTO 類中是否包含了名稱（Name）和電子郵件（EMail）屬性
			//string email = cmDto.Email;
			//new EMailHelper().SendConfirmRegisterEmail(url, name, email);

			return "註冊成功";
		}

		public bool ActiveRegister (int memberId, string confirmCode)
		{
			//驗證傳入值是否合理

			//if (memberId <= 0 || string.IsNullOrEmpty(confirmCode))
			//{
			//	return false; // 在view中,我們會顯示'已開通,謝謝'
			//}

			// 根據 Id, confirmCode 取得 未確認的 member
			Member member = _context.Members.FirstOrDefault(m => m.Id == memberId && m.IsConfirmed == false && m.ConfirmCode == confirmCode);
			if (member == null) return false;

			// 如果有找到, 將它更新為已確認
			member.IsConfirmed = true; // 視為已確認的會員
			member.ConfirmCode = null; // 清空 confirm code 欄位

			_context.SaveChanges();

			return true;
		}

		public string test(string word)
		{

			return _rsaService.Encrypt(word);
		}

		public string test2(string word)
		{

			return _rsaService.Decrypt(word);
		}


		//public string Encrypt(string password)
		//{
		//	// 加载密钥对
		//	RSAParameters publicKey;
		//	RSAParameters privateKey;
		//	RSAEncryptor.LoadKeyPair("publicKey.txt", "privateKey.txt", out publicKey, out privateKey);

		//	// 加密数据
		//	byte[] encryptedData = RSAEncryptor.Encrypt(Encoding.UTF8.GetBytes(password), publicKey);

		//	// 返回加密后的数据
		//	return Convert.ToBase64String(encryptedData);
		//}

		//public string Decrypt(string encryptedpassword)
		//{
		//	// 加载密钥对
		//	RSAParameters publicKey;
		//	RSAParameters privateKey;
		//	RSAEncryptor.LoadKeyPair("publicKey.txt", "privateKey.txt", out publicKey, out privateKey);

		//	// 解密数据
		//	byte[] decryptedData = RSAEncryptor.Decrypt(Convert.FromBase64String(encryptedpassword), privateKey);

		//	// 返回解密后的数据
		//	return Encoding.UTF8.GetString(decryptedData);
		//}

		//public void test()
		//{
		//	var rsaService = _rsaService;

		//	// 获取公钥
		//	string publicKey = rsaService.GetPublicKey();
		//	Console.WriteLine("Public Key:");
		//	Console.WriteLine(publicKey);

		//	// 加密示例
		//	string plainText = "Hello, world!";
		//	string encryptedText = rsaService.Encrypt(plainText);
		//	Console.WriteLine("Encrypted Text:");
		//	Console.WriteLine(encryptedText);

		//	// 解密示例
		//	string decryptedText = rsaService.Decrypt(encryptedText);
		//	Console.WriteLine("Decrypted Text:");
		//	Console.WriteLine(decryptedText);

		//	// 保存密钥示例
		//	rsaService.SavePrivateKeyToFile("private_key.xml");
		//	rsaService.SavePublicKeyToFile("public_key.xml");

		//}
	}
}
