using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthUsingCookies.Models
{
	public class User
	{
		public int Id { get; set; }

		[Required]
		[MinLength(3)]
		[MaxLength(100)]
		public string Username { get; set; }

		[Required]
		[MaxLength(250)]
		public string Email { get; set; }

		[Required]
		[MaxLength(1000)]
		public string PasswordHash { get; set; }

		[MaxLength(2000)]
		public string ImageName { get; set; }

		public ICollection<Role> Roles { get; set; }
	}
}
