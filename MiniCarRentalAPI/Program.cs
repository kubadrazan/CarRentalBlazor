
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using MiniCarRentalAPI.Data;
using Newtonsoft.Json.Converters;
using System.Text.Json.Serialization;

namespace MiniCarRentalAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.

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

            var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}
