using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;
using RGB.Back.DTOs;
using RGB.Back.Service;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.CodeAnalysis;
using Microsoft.IdentityModel.Tokens;
using RGB.Back.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using RGB.Back.Infra;



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
		private readonly JwtConfig _jwtConfig;


		public MembersController(IOptionsMonitor<JwtConfig> optionsMonitor,RizzContext context)
        {
			_context = context;
			_service = new MemberService(context);
			_jwtConfig = optionsMonitor.CurrentValue;


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

        [HttpPost("Login")]
		//LoginDTO loginDto
		//JwtPayload
		public async Task<List<string>> Login(LoginDTO loginDto)
		{
            var result= _service.ValidLogin(loginDto); // 驗證帳密是否ok,且是有效的會員
            var memberId = result.Item2.Id.ToString();
			var ava = result.Item2.AvatarUrl;
			var bouns = result.Item2.Bonus.ToString();
			var name = result.Item2.NickName;
			//JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();
			//呼叫GenerateJwtToken方法，建立jwtToken
			//AuthResult jwtToken = await GenerateJwtToken(memberId);
			//var token = jwtTokenHandler.ReadJwtToken(jwtToken.Token);
			//var payload = token.Payload;
			//回傳AuthResult
			//return payload;

			if (result.Item1 == false)
			{
				string errorMessage = "帳號或密碼錯誤";
				List<string> errors = new List<string>();
				errors.Add(errorMessage);
				return errors;
			}
			else
			{
				string sussceMessage = "登入成功";
				List<string> sussce = new List<string>();
				var protectId = _dataProtector.Protect(memberId);
				sussce.Add(sussceMessage);
				sussce.Add(protectId);
				sussce.Add(ava);
				sussce.Add(bouns);
				sussce.Add(name);
				sussce.Add(memberId);
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

		[HttpPost("TestMemberAccount")]
		public async Task<bool> TestMemberAccount(string account)
		{
			var member = _context.Members.FirstOrDefault(p => p.Account == account);
			if(member == null)
			{
			   return true;
			}
			else
			{
				return false;
			}

		}

		[HttpPost("TestMemberName")]
		public async Task<bool> TestMemberName(string name)
		{
			var member = _context.Members.FirstOrDefault(p => p.NickName == name);
			if (member == null)
			{
				return true;
			}
			else
			{
				return false;
			}
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
			//對程式
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

		//vue傳入Id google登入
		[HttpPost("GoogleId")]
		public async Task<List<string>> ValidGoogleId(string googleId)
		{
			Member member = _context.Members.FirstOrDefault(m => m.Google == googleId);
			if (member == null) {
				List<string> errors = new List<string>();
				errors.Add("新的google帳號");
				return errors;
			}
			//var memberId = result.Item2.Id.ToString();
			//var ava = result.Item2.AvatarUrl;
			//var bouns = result.Item2.Bonus.ToString();
			//var name = result.Item2.NickName;
			else
			{
			    var memberId = member.Id.ToString();
				var ava = member.AvatarUrl;
				var bouns = member.Bonus.ToString();
				var name = member.NickName;
				if (ava == null)
				{
					//預設頭像
					ava = "";
				}
				string sussceMessage = "登入成功";
				List<string> sussces = new List<string>();
				var protectId = _dataProtector.Protect(memberId);
				sussces.Add(sussceMessage);
				sussces.Add(protectId);
				sussces.Add(ava);
				sussces.Add(bouns);
				sussces.Add(name);
				return sussces;
			}
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


		/// <summary>
		/// 產生JWT Token
		/// </summary>
		/// <param name="user">User資料</param>
		/// <returns>AuthResult</returns>
		private async Task<AuthResult> GenerateJwtToken(string memberid)
		{
			#region 建立JWT Token
			//宣告JwtSecurityTokenHandler，用來建立token
			JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

			//appsettings中JwtConfig的Secret值
			byte[] key = Encoding.ASCII.GetBytes(_jwtConfig.Secret);

			//定義token描述
			SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
			{
				//設定要加入到 JWT Token 中的聲明資訊(Claims)
				Subject = new ClaimsIdentity(new[]
				{
		           new Claim(JwtRegisteredClaimNames.Iss, "RGB"),
				   new Claim("memberid", memberid)
				}),

				//設定Token的時效
				Expires = DateTime.UtcNow.AddSeconds(3600),

				//設定加密方式，key(appsettings中JwtConfig的Secret值)與HMAC SHA256演算法
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
			};

			//使用SecurityTokenDescriptor建立JWT securityToken
			SecurityToken token = jwtTokenHandler.CreateToken(tokenDescriptor);


			//token序列化為字串
			string jwtToken = jwtTokenHandler.WriteToken(token);
			#endregion

			//#region 回傳AuthResult
			return new AuthResult()
			{
				Token = jwtToken,
				Result = true,
				RefreshToken = RandomString(25) + Guid.NewGuid()
			};
			//#endregion
		}

		public static string RandomString(int length)
		{
			const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
			var random = new Random();
			var result = new StringBuilder(length);
			for (int i = 0; i < length; i++)
			{
				result.Append(chars[random.Next(chars.Length)]);
			}
			return result.ToString();
		}

		//[HttpGet("en")]
		//public async Task<string> test(string word)
		//{
			


		//	return _service.test(word); 
			

		//}

		//[HttpGet("un")]
		//public async Task<string> test2(string word)
		//{



		//	return _service.test2(word);


		//}



		/// <summary>
		/// 驗證Token，並重新產生Token
		/// </summary>
		/// <param name="tokenRequest">TokenRequest參數</param>
		/// <returns>AuthResult</returns>
		//private async Task<AuthResult> VerifyAndGenerateToken(TokenRequest tokenRequest)
		//{
		//	//建立JwtSecurityTokenHandler
		//	JwtSecurityTokenHandler jwtTokenHandler = new JwtSecurityTokenHandler();

		//	try
		//	{
		//		//驗證參數的Token，回傳SecurityToken
		//		ClaimsPrincipal tokenInVerification = jwtTokenHandler.ValidateToken(tokenRequest.Token, _tokenValidationParams, out SecurityToken validatedToken);

		//		if (validatedToken is JwtSecurityToken jwtSecurityToken)
		//		{
		//			//檢核Token的演算法
		//			var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

		//			if (result == false)
		//			{
		//				return null;
		//			}
		//		}


		//		//取Token Claims中的Iss(產生token時定義為Account)
		//		string JwtAccount = tokenInVerification.Claims.SingleOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss).Value;

		//		//檢核storedRefreshToken與JwtAccount的Account是否一致
		//		if (storedRefreshToken.Account != JwtAccount)
		//		{
		//			return new AuthResult()
		//			{
		//				Errors = new List<string>() { "Token驗證失敗" },
		//				Success = false
		//			};
		//		}

		//		//依storedRefreshToken的Account，查詢出DB的User資料
		//		User dbUser = _context.Users.Where(u => u.Account == storedRefreshToken.Account).FirstOrDefault();

		//		//產生Jwt Token
		//		return await GenerateJwtToken(dbUser);
		//	}
		//	catch (Exception ex)
		//	{
		//		return new AuthResult()
		//		{
		//			Success = false,
		//			Errors = new List<string>() {
		//		ex.Message
		//	}
		//		};
		//	}
		//}

		/// <summary>
		/// 驗證 Google 登入授權
		/// </summary>
		/// <returns></returns>
		//[HttpPost("LoginGoogle")]
		//public async Task<string> ValidGoogleLogin()
		//{
		//	string? formCredential = Request.Form["credential"]; //回傳憑證
		//	string? formToken = Request.Form["g_csrf_token"]; //回傳令牌
		//	string? cookiesToken = Request.Cookies["g_csrf_token"]; //Cookie 令牌
		//	string result = "";

		//	// 驗證 Google Token
		//	GoogleJsonWebSignature.Payload? payload = await VerifyGoogleToken(formCredential, formToken, cookiesToken);
		//	if (payload == null)
		//	{
		//		// 驗證失敗
		//		result = "驗證 Google 授權失敗";
		//	}
		//	else
		//	{
		//		//驗證成功，取使用者資訊內容
		//		result = "驗證 Google 授權成功" + "<br>";
		//		result += "Email:" + payload.Email + "<br>";
		//		result += "Name:" + payload.Name + "<br>";
		//		result += "Picture:" + payload.Picture;
		//	}

		//	return result;
		//}

		///// <summary>
		///// 驗證 Google Token
		///// </summary>
		///// <param name="formCredential"></param>
		///// <param name="formToken"></param>
		///// <param name="cookiesToken"></param>
		///// <returns></returns>

		//private async Task<GoogleJsonWebSignature.Payload?> VerifyGoogleToken(string? formCredential, string? formToken, string? cookiesToken)
		//{
		//	// 檢查空值
		//	if (formCredential == null || formToken == null && cookiesToken == null)
		//	{
		//		return null;
		//	}

		//	GoogleJsonWebSignature.Payload? payload;
		//	try
		//	{
		//		// 驗證 token
		//		if (formToken != cookiesToken)
		//		{
		//			return null;
		//		}

		//		// 驗證憑證
		//		IConfiguration Config = new ConfigurationBuilder().AddJsonFile("appSettings.json").Build();
		//		string GoogleApiClientId = Config.GetSection("GoogleApiClientId").Value;
		//		var settings = new GoogleJsonWebSignature.ValidationSettings()
		//		{
		//			Audience = new List<string>() { GoogleApiClientId }
		//		};
		//		payload = await GoogleJsonWebSignature.ValidateAsync(formCredential, settings);
		//		if (!payload.Issuer.Equals("accounts.google.com") && !payload.Issuer.Equals("https://accounts.google.com"))
		//		{
		//			return null;
		//		}
		//		if (payload.ExpirationTimeSeconds == null)
		//		{
		//			return null;
		//		}
		//		else
		//		{
		//			DateTime now = DateTime.Now.ToUniversalTime();
		//			DateTime expiration = DateTimeOffset.FromUnixTimeSeconds((long)payload.ExpirationTimeSeconds).DateTime;
		//			if (now > expiration)
		//			{
		//				return null;
		//			}
		//		}
		//	}
		//	catch
		//	{
		//		return null;
		//	}
		//	return payload;
		//}
	}
}
