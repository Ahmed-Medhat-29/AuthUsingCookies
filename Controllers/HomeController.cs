using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthUsingCookies.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		[Authorize]
		public IActionResult TestAuthentication()
		{
			return View();
		}

		[Authorize(Roles = "Admin")]
		public IActionResult TestAdminAuthority()
		{
			return View();
		}
	}
}
