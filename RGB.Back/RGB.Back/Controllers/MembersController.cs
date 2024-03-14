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
        public async Task<System.String> EditMember(int id, MemberDTO memberdto)
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
        [HttpPost]
        public async Task<string> CreateMember(MemberDTO memberdto)
        {
            var member = new Member()
            {
                Account = memberdto.Account,
                Password = memberdto.Password,
                Mail = memberdto.Mail,
                AvatarUrl = null,
                RegistrationDate = DateTime.Now,
                BanTime =null,
                Bonus = 0,
                LastLoginDate = DateTime.Now,
                Birthday = null
			};
            _context.Members.Add(member);
            await _context.SaveChangesAsync();

            return "註冊成功";
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
	}
}
