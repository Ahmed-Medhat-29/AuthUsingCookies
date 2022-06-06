using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthUsingCookies.Migrations
{
	public partial class AddImageNameColumnToUsersTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<string>(
				name: "ImageName",
				table: "Users",
				type: "nvarchar(2000)",
				maxLength: 2000,
				nullable: true);
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "ImageName",
				table: "Users");
		}
	}
}
