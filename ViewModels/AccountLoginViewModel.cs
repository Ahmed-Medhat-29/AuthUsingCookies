using System.ComponentModel.DataAnnotations;

namespace AuthUsingCookies.ViewModels
{
	public class AccountLoginViewModel
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and not more than 100")]
		public string Password { get; set; }

		public string ReturnUrl { get; set; }

		public bool RememberMe { get; set; }
	}
}
