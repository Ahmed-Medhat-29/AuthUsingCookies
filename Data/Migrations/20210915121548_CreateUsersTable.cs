﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace AuthUsingCookies.Migrations
{
	public partial class CreateUsersTable : Migration
	{
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Users",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
					Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
					PasswordHash = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Users", x => x.Id);
				});
		}

		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Users");
		}
	}
}
