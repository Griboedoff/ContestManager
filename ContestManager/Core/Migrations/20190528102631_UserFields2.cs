using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class UserFields2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "City",
                schema: "public",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Coach",
                schema: "public",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "City",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Coach",
                schema: "public",
                table: "Users");
        }
    }
}
