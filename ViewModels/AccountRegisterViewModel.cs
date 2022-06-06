using System.ComponentModel.DataAnnotations;

namespace AuthUsingCookies.ViewModels
{
	public class AccountRegisterViewModel
	{
		[Required]
		[StringLength(maximumLength: 100, MinimumLength = 3, ErrorMessage = "Username must be at least 3 characters and not more than 100")]
		public string Username { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; }

		[Required]
		[StringLength(maximumLength: 100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters and not more than 100")]
		public string Password { get; set; }

		[Required]
		[Compare(nameof(Password))]
		[Display(Name = "Password Confirmation")]
		public string PasswordConfirmation { get; set; }
	}
}
