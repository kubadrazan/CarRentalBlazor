
using Azure.Identity;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using MiniCarRentalAPI.Data;
using MiniCarRentalAPI.Services;
using Newtonsoft.Json.Converters;
using SharedDataModels;
using SharedDataModels.Factories;
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
			builder.Services.AddSwaggerGen(c =>
			{
				c.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Name = "X-Api-Key", 
					Type = SecuritySchemeType.ApiKey,
					Description = "API key needed to access the endpoints"
				});

				c.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "ApiKey"
							}
						},
						new string[] {}
					}
				});
			});

            // AzureKeyVault
            builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration.GetValue<string>("KeyVault:https")), new DefaultAzureCredential());

#if DEBUG
			builder.Services.AddDbContext<CarRentalContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
#else
			builder.Services.AddDbContext<CarRentalContext>(options =>
				options.UseSqlServer(builder.Configuration["CarRentalDBConnectionString"]));
#endif
			builder.Services.Configure<JsonOptions>(options =>
				options.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
			builder.Services.Configure<EmailServiceOptions>(options => options.APIKey = builder.Configuration["SendGridApiKey"]);
            builder.Services.Configure<AzureBlobServiceOptions>(options => options.ConnectionString = builder.Configuration["AzureBlobConnectionString"]);
			builder.Services.AddSingleton(TimeProvider.System);
            builder.Services.AddTransient<EmailService>();
			builder.Services.AddTransient<AcceptationFactory>();
			builder.Services.AddTransient<OfferFactory>();
			builder.Services.AddTransient<ReturnFactory>();
			builder.Services.AddTransient<RentalFactory>();
			builder.Services.AddTransient<PdfGenerationService>();
            builder.Services.AddTransient<AzureBlobService>();

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
