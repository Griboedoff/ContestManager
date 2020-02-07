using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class ParticipantAuditorium : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Auditorium",
                schema: "public",
                table: "Participants",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Auditorium",
                schema: "public",
                table: "Participants");
        }
    }
}
