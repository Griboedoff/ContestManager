using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class Seating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Login",
                schema: "public",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Pass",
                schema: "public",
                table: "Participants",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AuditoriumsJson",
                schema: "public",
                table: "Contests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Login",
                schema: "public",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "Pass",
                schema: "public",
                table: "Participants");

            migrationBuilder.DropColumn(
                name: "AuditoriumsJson",
                schema: "public",
                table: "Contests");
        }
    }
}
