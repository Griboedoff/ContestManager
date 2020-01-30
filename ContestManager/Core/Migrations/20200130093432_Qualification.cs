using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class Qualification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualificationParticipations",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContestId = table.Column<Guid>(nullable: false),
                    ParticipantId = table.Column<Guid>(nullable: false),
                    EndTime = table.Column<DateTimeOffset>(nullable: false),
                    SerializedAnswers = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationParticipations", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualificationParticipations",
                schema: "public");
        }
    }
}
