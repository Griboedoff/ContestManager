using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class Options : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "State",
                schema: "public",
                table: "Contests");

            migrationBuilder.AddColumn<int>(
                name: "Options",
                schema: "public",
                table: "Contests",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Options",
                schema: "public",
                table: "Contests");

            migrationBuilder.AddColumn<int>(
                name: "State",
                schema: "public",
                table: "Contests",
                nullable: false,
                defaultValue: 0);
        }
    }
}
