using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class ContestRefactoring : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MdContent",
                schema: "public",
                table: "News",
                newName: "Title");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                schema: "public",
                table: "News",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                schema: "public",
                table: "News");

            migrationBuilder.RenameColumn(
                name: "Title",
                schema: "public",
                table: "News",
                newName: "MdContent");
        }
    }
}
