using System;
using Core.DataBaseEntities;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class QualTasks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "QualificationTasks",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    Text = table.Column<string>(nullable: true),
                    Answer = table.Column<string>(nullable: true),
                    ForClasses = table.Column<Class[]>(nullable: true),
                    ContestId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_QualificationTasks", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "QualificationTasks",
                schema: "public");
        }
    }
}
