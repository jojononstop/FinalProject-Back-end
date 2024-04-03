using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace RGB.Back.Service
{
	public class JWTservice
	{
		//包裝jwt
		public string GenerateJWT(string secretKey, params Claim[] claims)
		{
			//憑證
			var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				//發行者
				issuer: "RGB",
				//接受者
				audience: "audience",
				//聲明:想要傳內容
				claims: claims,
				//過期時間
				expires: DateTime.UtcNow.AddMinutes(60),
				//簽署憑證
				signingCredentials: credentials
			);

			var tokenHandler = new JwtSecurityTokenHandler();
			var encryptedToken = tokenHandler.WriteToken(token);

			return encryptedToken;
		}


		//拆jwt
		public ClaimsPrincipal DecodeJWT(string jwtToken, SecurityKey securityKey)
		{
			var tokenHandler = new JwtSecurityTokenHandler();

			// 定義 TokenValidationParameters 用於驗證 JWT
			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true, 
				ValidIssuer = "RGB",

				ValidateAudience = false, // 不驗證發行者

				ValidateIssuerSigningKey = true,
				IssuerSigningKey = securityKey,

				RequireExpirationTime = true,
				ValidateLifetime = true
			};

			// 使用 JwtSecurityTokenHandler 解析並驗證 JWT
			try
			{
				var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
				return claimsPrincipal;
			}
			catch (SecurityTokenValidationException)
			{
				// JWT 驗證失敗
				return null;
			}
		}

	}
}
