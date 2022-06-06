using AuthUsingCookies.Data;
using AuthUsingCookies.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthUsingCookies
{
	public class Startup
	{
		private readonly IConfiguration _config;

		public Startup(IConfiguration config)
		{
			_config = config;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();

			services.AddDbContextPool<ApplicationDbContext>(options =>
				options.UseSqlServer(_config.GetConnectionString("DefaultConnection")));

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.HttpOnly = false;
					options.Cookie.IsEssential = true;
				});

			services.AddControllersWithViews();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();

			app.UseStaticFiles();
			app.UseRouting();
			app.UseAuthentication();
			app.UseAuthorization();
			app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());
		}
	}
}