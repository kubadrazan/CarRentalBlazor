using Azure.Identity;
using Browser_FrontEnd.Services;
using Hangfire;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Server;
using Microsoft.EntityFrameworkCore;
using MiNICarRentalBrowser.Components;
using MiNICarRentalBrowser.Data;
using MiNICarRentalBrowser.Services;
using MiNICarRentalBrowser.Services.Car_Service;
using MudBlazor.Services;

namespace MiNICarRentalBrowser
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			// AzureKeyVault
			builder.Configuration.AddAzureKeyVault(new Uri(builder.Configuration.GetValue<string>("KeyVault:https")), new DefaultAzureCredential());
			builder.Services.AddDbContext<UsersContext>(options =>
				options.UseSqlServer(builder.Configuration["UsersDBConnectionString"]));

			builder.Services.AddScoped<UserValidationService>();
			builder.Services.AddScoped<EmployeeValidationService>();

			// Google Authentication
			builder.Services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
			})
			.AddCookie()
			.AddGoogle(googleOptions =>
			{
				googleOptions.ClientId = builder.Configuration["GoogleClientId"];
				googleOptions.ClientSecret = builder.Configuration["GoogleClientSecret"];
			});
			builder.Services.AddAuthorization(options =>
			{
				options.AddPolicy("RegisteredPolicy", policy =>
					policy.Requirements.Add(new RegisteredUserRequirement()));
				options.AddPolicy("EmployeePolicy", policy =>
					policy.Requirements.Add(new EmployeeRoleRequirement()));
			});

			builder.Services.AddScoped<IAuthorizationHandler, RegistrationHandler>();
			builder.Services.AddScoped<IAuthorizationHandler, EmployeeRoleHandler>();
			builder.Services.AddScoped<AuthenticationStateProvider, ServerAuthenticationStateProvider>();
			builder.Services.AddHttpContextAccessor();

            builder.Services.AddScoped<ICarRepository, CarRepository>();

			builder.Services.AddHangfire(config =>
			{
				config.UseSqlServerStorage(builder.Configuration["UsersDBConnectionString"]);
			});

			builder.Services.AddHangfireServer();

            // Add services to the container.
            builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();
			builder.Services.AddMudServices();

			builder.Services.AddSingleton<ApiKeyProvider>();
            builder.Services.AddTransient<CustomHttpMessageHandler>();

            builder.Services.AddHttpClient("ApiKeyClient")
					.AddHttpMessageHandler<CustomHttpMessageHandler>();

			builder.Services.AddScoped<RentalServicecs>();
			builder.Services.AddScoped<IUserService, UserServices>();

			builder.Services.AddScoped<ICarRental, CarRentalA>();
			builder.Services.AddScoped<AggregatedCarService>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

            app.UseStatusCodePagesWithReExecute("/error-page/{0}");

            app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.UseAuthentication();
			app.UseAuthorization();

            app.UseHangfireDashboard();

            app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();

			RecurringJob.AddOrUpdate<AggregatedCarService>(
				"update-car-data",
				service => service.UpdateCarsInDBAsync(),
                "*/30 * * * *"
                );
			app.Run();
		}
	}
}
