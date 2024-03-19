using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;
using System.Web;
using RGB.Back.DTOs;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using NuGet.Common;
using RGB.Back.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.DependencyInjection;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Google.Apis.Auth;
using System.Diagnostics.Metrics;



namespace RGB.Back.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly RizzContext _context;
		private readonly MemberService _service;
		private readonly IDataProtector _dataProtector;


		public MembersController(RizzContext context)
        {
			_context = context;
			_service = new MemberService(context);

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

        // GET: api/Members
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        //{
        //    return await _context.Members.ToListAsync();
        //}

        //// GET: api/Members/5
        //[HttpGet("{id}")]
        //public async Task<Member> GetMember(int id)
        //{
        //    var member = await _context.Members.FindAsync(id);

        //    //if (member == null)
        //    //{
        //    //    return NotFound();
        //    //}

        //    return member;
        //}

        [HttpPost("Login")]
        //LoginDTO loginDto
        public async Task<List<string>> Login(LoginDTO loginDto)
		{
            var result= _service.ValidLogin(loginDto); // 驗證帳密是否ok,且是有效的會員
            var memberId = result.Item2.ToString();
            if (result.Item1 == false)
            {
				string errorMessage = "帳號或密碼錯誤";
                string id = "0";
				List<string> errors = new List<string>();
                errors.Add(errorMessage);
				errors.Add(id);
				return errors;
            }
            else 
            {
				string sussceMessage = "登入成功";
				List<string> sussce = new List<string>();
				var protectId = _dataProtector.Protect(memberId);
				sussce.Add(sussceMessage);
				sussce.Add(protectId);

				//ProcessLogin(memberId);
                return sussce;
			}
		}

		[HttpPost("MemberId")]
		public async Task<string> MemberId(string protectId)
        {
			var Id = _dataProtector.Unprotect(protectId);
			return Id;
		}


		[HttpPost("2")]
		public System.String Logout(LoginDTO loginDto)
		{

			DeleteCookie(); // 刪除cookie

			return "登出成功"; // 轉向到它原本應該要去的網址
		}


		// PUT: api/Members/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//修改
		[HttpPut("{id}")]
        public async Task<string> EditMember(int id, MemberDTO memberdto)
        {
            if (id != memberdto.Id)
            {
                return "編輯失敗";
            }

            _context.Entry(memberdto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MemberExists(id))
                {
                    return "編輯失敗";
                }
                else
                {
                    throw;
                }
            }

            return "編輯成功";
        }

        // POST: api/Members
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //新增
        [HttpPost("Create")]
        public async Task<string> CreateMember(CreateMemberDTO cmDto)
        {
			//var urlTemplate = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/Members";
			var result= _service.CreateMember(cmDto);

            return result;
        }

        // DELETE: api/Members/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMember(int id)
        //{
        //    var member = await _context.Members.FindAsync(id);
        //    if (member == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Members.Remove(member);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }

        //      //將使用者資訊存入cookie
        private void ProcessLogin(string memberId)
        {
            CookieOptions options = new CookieOptions();
            // 设置过期时间
            options.Expires = DateTime.Now.AddHours(1);
			var protectText = _dataProtector.Protect(memberId);
			var unprotectText = _dataProtector.Unprotect(protectText);
			HttpContext.Response.Cookies.Append("Account", protectText, options);
            HttpContext.Response.Cookies.Append("AccountUN", unprotectText, options);
        }

        //刪除cookie
        private void DeleteCookie()
		{
			HttpContext.Response.Cookies.Delete("Account");
			//HttpContext.Response.Cookies.Delete("AccountUN");
		}

		/// <summary>
		/// 驗證 Google 登入授權
		/// </summary>
		/// <returns></returns>
		[HttpPost("LoginGoogle")]
		public async Task<string> ValidGoogleLogin()
		{
			string? formCredential = Request.Form["credential"]; //回傳憑證
			string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
			string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌
			string result = "";

			// 驗證 Google Token
			GoogleJsonWebSignature.Payload? payload = await VerifyGoogleToken(formCredential, formToken, cookiesToken);
			if (payload == null)
			{
				// 驗證失敗
				result = "驗證 Google 授權失敗";
			}
			else
			{
				//驗證成功，取使用者資訊內容
				result = "驗證 Google 授權成功" + "<br>";
				result += "Email:" + payload.Email + "<br>";
				result += "Name:" + payload.Name + "<br>";
				result += "Picture:" + payload.Picture;
			}

			return result;
		}

		/// <summary>
		/// 驗證 Google Token
		/// </summary>
		/// <param name="formCredential"></param>
		/// <param name="formToken"></param>
		/// <param name="cookiesToken"></param>
		/// <returns></returns>

		private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
		{
			// 檢查空值
			if (formCredential == null || formToken == null && cookiesToken == null)
			{
				return null;
			}

			GoogleJsonWebSignature.Payload? payload;
			try
			{
				// 驗證 token
				if (formToken != cookiesToken)
				{
					return null;
				}

				// 驗證憑證
				IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
				string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
				var settings = new GoogleJsonWebSignature.ValidationSettings()
				{
					Audience = new List<string>() { GoogleApiClientId }
				};
				payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
				if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
				{
					return null;
				}
				if (payload.ExpirationTimeSeconds == null)
				{
					return null;
				}
				else
				{
					DateTime now = DateTime.Now.ToUniversalTime();
					DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
					if (now > expiration)
					{
						return null;
					}
				}
			}
			catch
			{
				return null;
			}
			return payload;
		}

		//確認郵箱
		[HttpPost("ActiveRegister")]
		public async Task<string> ActiveRegister(int id, string confirmCode)
		{
			var result = _service.ActiveRegister(id, confirmCode);
			if (result == false)
			{
				return "驗證失敗";
			}
			else
			{
				return "已通過驗證";
			}
		}


	}
}
