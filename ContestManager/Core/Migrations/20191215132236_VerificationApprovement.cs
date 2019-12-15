using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class VerificationApprovement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Verified",
                schema: "public",
                table: "Participants",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Verified",
                schema: "public",
                table: "Participants");
        }
    }
}
