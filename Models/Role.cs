using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace AuthUsingCookies.Models
{
	public class Role
	{
		public int Id { get; set; }

		[Required]
		[MaxLength(25)]
		public string Name { get; set; }

		public ICollection<User> Users { get; set; }
	}
}
