using Browser_FrontEnd.Services;
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
using MudBlazor.Services;

namespace MiNICarRentalBrowser
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddDbContext<UsersContext>(options =>
				options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
				);

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
				googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"];
				googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
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


			// Add services to the container.
			builder.Services.AddRazorComponents()
				.AddInteractiveServerComponents();
			builder.Services.AddMudServices();

			builder.Services.AddHttpClient("ApiKeyClient")
					.AddHttpMessageHandler<CustomHttpMessageHandler>();

			builder.Services.AddTransient<CustomHttpMessageHandler>();

			builder.Services.AddScoped<RentalServicecs>();
			builder.Services.AddScoped<IUserService, UserServices>();

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
			{
				app.UseExceptionHandler("/Error");
				// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
				app.UseHsts();
			}

			app.UseHttpsRedirection();

			app.UseStaticFiles();
			app.UseAntiforgery();

			app.UseAuthentication();
			app.UseAuthorization();

			app.MapRazorComponents<App>()
				.AddInteractiveServerRenderMode();



			app.Run();
		}
	}
}
