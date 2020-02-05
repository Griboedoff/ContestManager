using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class JsonColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerializedUserSnapshot",
                schema: "public",
                table: "Participants",
                type: "json",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "SerializedUserSnapshot",
                schema: "public",
                table: "Participants",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "json",
                oldNullable: true);
        }
    }
}
