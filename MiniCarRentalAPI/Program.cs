
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using Newtonsoft.Json.Converters;
using SharedDataModels;
using System.Text.Json.Serialization;

namespace MiniCarRentalAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);


			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			// TODO Change to AddDbContextFactory??
			builder.Services.AddDbContext<CarRentalContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
			);
			builder.Services.Configure<JsonOptions>(options =>
				options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
			builder.Services.Configure<EmailServiceOptions>(options => options.APIKey = builder.Configuration["EmailService:SendGrid:ApiKey"]);
			builder.Services.AddTransient<EmailService>();
			builder.Services.AddTransient<IApiKeyValidatorService, ApiKeyValidatorService>();
			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();
			app.UseMiddleware<ApiAuthMiddleware>();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
