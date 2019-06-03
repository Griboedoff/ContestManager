using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class ContestResults : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResultsTableLink",
                schema: "public",
                table: "Contests",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TasksDescriptionJson",
                schema: "public",
                table: "Contests",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResultsTableLink",
                schema: "public",
                table: "Contests");

            migrationBuilder.DropColumn(
                name: "TasksDescriptionJson",
                schema: "public",
                table: "Contests");
        }
    }
}
