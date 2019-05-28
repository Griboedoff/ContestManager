using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class UserSnapshot : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerializedUserSnapshot",
                schema: "public",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedUserSnapshot",
                schema: "public",
                table: "Participants");
        }
    }
}
