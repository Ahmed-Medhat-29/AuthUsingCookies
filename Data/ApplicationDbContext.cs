using AuthUsingCookies.Models;
using Microsoft.EntityFrameworkCore;

namespace AuthUsingCookies.Data
{
	public class ApplicationDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<Role> Roles { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
		{

		}
	}
}
