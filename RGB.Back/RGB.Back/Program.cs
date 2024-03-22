
using Microsoft.EntityFrameworkCore;
using RGB.Back.Models;
using RGB.Back.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace RGB.Back
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			//jwt

			builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("JwtConfig"));


			var key = Encoding.ASCII.GetBytes(builder.Configuration["JwtConfig:Secret"]);

			TokenValidationParameters tokenValidationParams = new TokenValidationParameters
			{
				RequireExpirationTime = false,
				ValidateIssuer = false,
				ValidateAudience = false,

				//驗證IssuerSigningKey
				ValidateIssuerSigningKey = true,
				//以JwtConfig:Secret為Key，做為Jwt加密
				IssuerSigningKey = new SymmetricSecurityKey(key),

				//驗證時效
				ValidateLifetime = true,

				//設定token的過期時間可以以秒來計算，當token的過期時間低於五分鐘時使用。
				ClockSkew = TimeSpan.Zero
			};

			//註冊tokenValidationParams，後續可以注入使用。
			builder.Services.AddSingleton(tokenValidationParams);

			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			})
			.AddJwtBearer(jwt =>
			{
				jwt.SaveToken = true;
				jwt.TokenValidationParameters = tokenValidationParams;
			});
			//以上jwt
			// Add services to the container.
			builder.Services.AddDbContext<RizzContext>(options =>
			{
				options.UseSqlServer(builder.Configuration.GetConnectionString("Rizz"));
			});

			string CorsPolicy = "AllowAny";
			builder.Services.AddCors(options =>
			{
				options.AddPolicy(
					name: CorsPolicy,
					policy =>
					{
						policy.WithOrigins("*").WithHeaders("*").WithMethods("*");
					});
			});

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors();
			app.UseHttpsRedirection();
			//啟用身分識別
			app.UseAuthentication();
			//啟用授權功能
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}

	}
}
