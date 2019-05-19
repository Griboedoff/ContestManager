using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class userFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SerializedFields",
                schema: "public",
                table: "Users",
                newName: "School");

            migrationBuilder.AddColumn<int>(
                name: "Class",
                schema: "public",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Sex",
                schema: "public",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                schema: "public",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Sex",
                schema: "public",
                table: "Users");

            migrationBuilder.RenameColumn(
                name: "School",
                schema: "public",
                table: "Users",
                newName: "SerializedFields");
        }
    }
}
