using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AuthUsingCookies.Data;
using AuthUsingCookies.Models;
using AuthUsingCookies.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthUsingCookies.Controllers
{
	public class AccountController : Controller
	{
		private readonly ApplicationDbContext _context;
		private readonly IPasswordHasher<User> _hasher;

		public AccountController(ApplicationDbContext context, IPasswordHasher<User> hasher)
		{
			_context = context;
			_hasher = hasher;
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Index()
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var id = int.Parse(userId);
			var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
			return View(user);
		}

		[HttpGet]
		public IActionResult Login(string returnUrl)
		{
			var model = new AccountLoginViewModel
			{
				ReturnUrl = returnUrl
			};
			return View(model);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Login(AccountLoginViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			var user = await _context.Users.Include(u => u.Roles).FirstOrDefaultAsync(m => m.Email == model.Email);
			if (user == null)
			{
				ModelState.AddModelError("", "Email not found, Please register first");
				return View(model);
			}

			var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, model.Password);
			if (result == PasswordVerificationResult.Failed)
			{
				ModelState.AddModelError("", "Invalid login attempt");
				return View(model);
			}

			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
				new Claim(ClaimTypes.Name, user.Username)
			};

			foreach (var role in user.Roles)
				claims.Add(new Claim(ClaimTypes.Role, role.Name));

			ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
			ClaimsPrincipal principal = new ClaimsPrincipal(identity);
			AuthenticationProperties properties = new AuthenticationProperties
			{
				IsPersistent = model.RememberMe,
				ExpiresUtc = System.DateTimeOffset.Now.AddDays(14)
			};
			await HttpContext.SignInAsync(principal, properties);

			if (!string.IsNullOrWhiteSpace(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
				return Redirect(model.ReturnUrl);

			return RedirectToAction("Index", "Home");
		}

		[HttpGet]
		public IActionResult Register()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Register(AccountRegisterViewModel model)
		{
			if (!ModelState.IsValid)
				return View(model);

			if (_context.Users.Any(m => m.Email == model.Email))
			{
				ModelState.AddModelError("", "Email is already registered");
				return View(model);
			}

			var user = new User
			{
				Username = model.Username,
				Email = model.Email
			};

			user.PasswordHash = _hasher.HashPassword(user, model.Password);

			await _context.Users.AddAsync(user);
			if (await _context.SaveChangesAsync() < 1)
			{
				ModelState.AddModelError("", "Something wrong happened");
				return View(model);
			}

			return RedirectToAction(nameof(Login));
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Logout()
		{
			await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}

		public IActionResult AccessDenied()
		{
			return View();
		}

		[HttpPost]
		[Authorize]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(User user, IFormFile image)
		{
			var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
			var id = int.Parse(userId);
			var userdb = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);

			if (string.IsNullOrWhiteSpace(user.Username))
			{
				ModelState.AddModelError("", "The Username field is required");
				return View("Index", userdb);
			}

			var path = "wwwroot/img/user/";
			var fileName = userdb.Id + Path.GetExtension(image.FileName);
			using (var stream = System.IO.File.Create(path + fileName))
				image.CopyTo(stream);

			userdb.ImageName = fileName;
			_context.SaveChanges();
			return View("Index", userdb);
		}
	}
}