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

namespace RGB.Back.Controllers
{
    [EnableCors("AllowAny")]
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        private readonly RizzContext _context;
		private readonly MemberService _service;

		public MembersController(RizzContext context)
        {
            _context = context;
			_service = new MemberService(context);
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
        public async Task<string> Login(LoginDTO loginDto)
		{
            var result= _service.ValidLogin(loginDto); // 驗證帳密是否ok,且是有效的會員
            if (result == false)
            {
                return "帳號或密碼錯誤";
            }
            else 
            {
				return "登入成功";
			}
		}

		[HttpPost("2")]
		public String Logout(LoginDTO loginDto)
		{

			DeleteCookie(); // 刪除cookie

			return "登出成功"; // 轉向到它原本應該要去的網址
		}


		// PUT: api/Members/5
		// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
		//修改
		[HttpPut("{id}")]
        public async Task<String> EditMember(int id, MemberDTO memberdto)
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
		//private void ProcessLogin(LoginDTO LoginDto)
		//{
		//	CookieOptions options = new CookieOptions();
		//	// 设置过期时间
		//	options.Expires = DateTime.Now.AddHours(1);
		//	HttpContext.Response.Cookies.Append("Account", LoginDto.Account, options);
		//}

		//刪除cookie
		private void DeleteCookie()
		{
			HttpContext.Response.Cookies.Delete("Account");
		}
	}
}
