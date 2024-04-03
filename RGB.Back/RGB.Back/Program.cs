

using Microsoft.AspNetCore.Http.Connections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;
using RGB.Back.Hubs;
using RGB.Back.Models;
using RGB.Back.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using RGB.Back.Service;



namespace RGB.Back
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<RizzContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("Rizz"));
            });

           
			

			builder.Services.AddSignalR()
							.AddJsonProtocol(options =>
							{
											  options.PayloadSerializerOptions.PropertyNamingPolicy = null;
							});

            builder.Services.AddScoped<ChatService>();
            //builder.Services.AddScoped<CartService>();
            //builder.Services.AddScoped<GameService>();


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
			//啟用身分識別
			app.UseAuthentication();
			//啟用授權功能
			app.UseCors(CorsPolicy);
          
            app.UseHttpsRedirection();

			app.UseDefaultFiles();
			app.UseStaticFiles();

			app.MapHub<ChatHub>("/chatHub", options =>
			{
				options.Transports = HttpTransportType.WebSockets | HttpTransportType.LongPolling;
			});
            app.MapControllers();

            app.Run();
		}
        }
    }

