using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class PassRecovery : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "PasswordRestore",
                schema: "public",
                table: "EmailConfirmationRequests",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordRestore",
                schema: "public",
                table: "EmailConfirmationRequests");
        }
    }
}
