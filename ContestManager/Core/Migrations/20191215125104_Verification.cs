using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class Verification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Verification",
                schema: "public",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verification",
                schema: "public",
                table: "Participants");
        }
    }
}
