using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class RemoveFieldsFromContest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SerializedFields",
                schema: "public",
                table: "Contests");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "SerializedFields",
                schema: "public",
                table: "Contests",
                nullable: true);
        }
    }
}
