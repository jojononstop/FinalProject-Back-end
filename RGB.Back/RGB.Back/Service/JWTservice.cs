using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Serialization;
using Newtonsoft.Json;
using RGB.Back.Infra;
using System.Text.Json.Serialization;
using System.Text.Json;


namespace RGB.Back.Service
{
	public class JWTservice
	{
		private static RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
		// 加载预先生成的密钥对文件

		private RSAParameters _privateKey;
		private RSAParameters _publicKey;


		//public RSAEncryptor()
		//{
		//	_privateKey = rsa.ExportParameters(true);
		//	_publicKey = rsa.ExportParameters(false);

		//}

		public JWTservice(string privateKeyFilePath, string publicKeyFilePath)
		{
			if (File.Exists(privateKeyFilePath) && File.Exists(publicKeyFilePath))
			{
				using (StreamReader srPrivate = new StreamReader(privateKeyFilePath))
				{
					var xsPrivate = new XmlSerializer(typeof(RSAParameters));
					_privateKey = (RSAParameters)xsPrivate.Deserialize(srPrivate);
				}

				using (StreamReader srPublic = new StreamReader(publicKeyFilePath))
				{
					var xsPublic = new XmlSerializer(typeof(RSAParameters));
					_publicKey = (RSAParameters)xsPublic.Deserialize(srPublic);
				}

				rsa = new RSACryptoServiceProvider();
				rsa.ImportParameters(_privateKey);
			}
			else
			{
				throw new FileNotFoundException("Private or public key file not found.");
			}
		}

		//包裝jwt

		public string GenerateJWT(params Claim[] claims)
		{
			// 创建一个 RSA 签名对象，使用已经导入的私钥
			var rsaSigner = new RSACryptoServiceProvider();
			rsaSigner.ImportParameters(_privateKey);

			// 创建签名凭据
			var securityKey = new RsaSecurityKey(rsaSigner);
			var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.RsaSha256);

			var claimList = new List<Claim>();
			foreach (var claim in claims)
			{
				claimList.Add(claim);
			}


			// 创建 JWT
			var token = new JwtSecurityToken(
				issuer: "RGB",
				audience: "audience",
				claims: claimList,
				expires: DateTime.UtcNow.AddMinutes(60),
				signingCredentials: credentials
			);

			var tokenHandler = new JwtSecurityTokenHandler();
			var encryptedToken = tokenHandler.WriteToken(token);

			return encryptedToken;
		}

		//public string GenerateJWT( params Claim[] claims)
		//{
		//	string publicKeyString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_publicKey)));
		//	//憑證
		//	var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(publicKeyString));

		//	var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

		//	// 创建一个空的声明列表
		//	var claimList = new List<Claim>();

		//	// 将传入的声明添加到声明列表中
		//	foreach (var claim in claims)
		//	{
		//		claimList.Add(claim);
		//	}

		//	var token = new JwtSecurityToken(
		//		//發行者
		//		issuer: "RGB",
		//		//接受者
		//		audience: "audience",
		//		//聲明:想要傳內容
		//		claims: claimList,
		//		//過期時間
		//		expires: DateTime.UtcNow.AddMinutes(60),
		//		//簽署憑證
		//		signingCredentials: credentials
		//	);

		//	var tokenHandler = new JwtSecurityTokenHandler();
		//	var encryptedToken = tokenHandler.WriteToken(token);

		//	return encryptedToken;
		//}


		//拆jwt

		//public ClaimsPrincipal DecodeJWT(string jwtToken)
		//{
		//	string privateKeyString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_privateKey)));
		//	var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(privateKeyString));
		//	var tokenHandler = new JwtSecurityTokenHandler();

		//	// 定义 TokenValidationParameters 用于验证 JWT
		//	var validationParameters = new TokenValidationParameters
		//	{
		//		ValidateIssuer = true,
		//		ValidIssuer = "RGB",

		//		ValidateAudience = false, // 不验证发行者

		//		ValidateIssuerSigningKey = true,
		//		IssuerSigningKey = securityKey,

		//		RequireExpirationTime = true,
		//		ValidateLifetime = true
		//	};

		public string DecodeAndSerializeClaims(string jwtToken)
		{
			// 创建一个 RSA 签名对象，使用已经导入的私钥
			var rsaSigner = new RSACryptoServiceProvider();
			rsaSigner.ImportParameters(_publicKey);

			var securityKey = new RsaSecurityKey(rsaSigner); // 创建RSA安全密钥

			var tokenHandler = new JwtSecurityTokenHandler();

			var validationParameters = new TokenValidationParameters
			{
				ValidateIssuer = true,
				ValidIssuer = "RGB",

				ValidateAudience = true,
				ValidAudience = "audience",

				ValidateIssuerSigningKey = true,
				IssuerSigningKey = securityKey,

				RequireExpirationTime = true,
				ValidateLifetime = true
			};

			try
			{
				var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);

				// 只序列化 ClaimsPrincipal 的主体声明
				var serializedClaims = SerializeClaimsPrincipal(claimsPrincipal);

				return serializedClaims;
			}
			catch (SecurityTokenValidationException)
			{
				// JWT 验证失败
				return null;
			}
		}

		private string SerializeClaimsPrincipal(ClaimsPrincipal claimsPrincipal)
		{
			// 提取主体声明 (Claims)
			var claims = claimsPrincipal.Claims;

			// 创建匿名对象，仅包含主体声明的属性
			var anonymousObject = new
			{
				Claims = claims.Select(c => new { c.Type, c.Value })
			};

			// 使用 JsonSerializerOptions 对象序列化匿名对象
			var serializerOptions = new JsonSerializerOptions
			{
				ReferenceHandler = ReferenceHandler.Preserve,
				MaxDepth = 64 // 增加最大深度，根据需要调整
			};

			var serializedClaims = System.Text.Json.JsonSerializer.Serialize(anonymousObject, serializerOptions);
			return serializedClaims;
		}

		//public ClaimsPrincipal DecodeJWT(string jwtToken)
		//{
		//	// 创建一个 RSA 签名对象，使用已经导入的私钥
		//	var rsaSigner = new RSACryptoServiceProvider();
		//	rsaSigner.ImportParameters(_publicKey);

		//	var securityKey = new RsaSecurityKey(rsaSigner); // 创建RSA安全密钥

		//	var tokenHandler = new JwtSecurityTokenHandler();

		//	var validationParameters = new TokenValidationParameters
		//	{
		//		ValidateIssuer = true,
		//		ValidIssuer = "RGB",

		//		ValidateAudience = true,
		//		ValidAudience = "audience",

		//		ValidateIssuerSigningKey = true,
		//		IssuerSigningKey = securityKey,

		//		RequireExpirationTime = true,
		//		ValidateLifetime = true
		//	};

		//	try
		//	{
		//		var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
		//		return claimsPrincipal;
		//	}
		//	catch (SecurityTokenValidationException)
		//	{
		//		// JWT 验证失败
		//		return null;
		//	}
		//}


		//public ClaimsPrincipal DecodeJWT(string jwtToken)
		//{
		//	string publicKeyString = Convert.ToBase64String(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(_publicKey)));
		//	var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(publicKeyString));
		//	var tokenHandler = new JwtSecurityTokenHandler();

		//	// 定義 TokenValidationParameters 用於驗證 JWT
		//	var validationParameters = new TokenValidationParameters
		//	{
		//		ValidateIssuer = true, 
		//		ValidIssuer = "RGB",

		//		ValidateAudience = false, // 不驗證發行者

		//		ValidateIssuerSigningKey = true,
		//		IssuerSigningKey = securityKey,

		//		RequireExpirationTime = true,
		//		ValidateLifetime = true
		//	};

		//	// 使用 JwtSecurityTokenHandler 解析並驗證 JWT
		//	try
		//	{
		//		var claimsPrincipal = tokenHandler.ValidateToken(jwtToken, validationParameters, out var validatedToken);
		//		return claimsPrincipal;
		//	}
		//	catch (SecurityTokenValidationException)
		//	{
		//		// JWT 驗證失敗
		//		return null;
		//	}
		//}

	}
}
