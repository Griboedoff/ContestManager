using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "public");

            migrationBuilder.CreateTable(
                name: "AuthenticationAccounts",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    ServiceId = table.Column<string>(maxLength: 100, nullable: true),
                    ServiceToken = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthenticationAccounts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contests",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(nullable: true),
                    OwnerId = table.Column<Guid>(nullable: false),
                    Options = table.Column<int>(nullable: false),
                    State = table.Column<int>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    SerializedFields = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfirmationRequests",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    ConfirmationCode = table.Column<string>(maxLength: 7, nullable: true),
                    IsUsed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfirmationRequests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContestId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    SerializedResults = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StoredConfigs",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    TypeName = table.Column<string>(nullable: true),
                    JsonValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoredConfigs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(maxLength: 100, nullable: true),
                    Role = table.Column<int>(nullable: false),
                    SerializedFields = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                schema: "public",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ContestId = table.Column<Guid>(nullable: false),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    MdContent = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                    table.ForeignKey(
                        name: "FK_News_Contests_ContestId",
                        column: x => x.ContestId,
                        principalSchema: "public",
                        principalTable: "Contests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AuthenticationAccounts_Type_ServiceId",
                schema: "public",
                table: "AuthenticationAccounts",
                columns: new[] { "Type", "ServiceId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contests_Title",
                schema: "public",
                table: "Contests",
                column: "Title",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmationRequests_Type_Email_ConfirmationCode",
                schema: "public",
                table: "EmailConfirmationRequests",
                columns: new[] { "Type", "Email", "ConfirmationCode" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_News_ContestId",
                schema: "public",
                table: "News",
                column: "ContestId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AuthenticationAccounts",
                schema: "public");

            migrationBuilder.DropTable(
                name: "EmailConfirmationRequests",
                schema: "public");

            migrationBuilder.DropTable(
                name: "News",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Participants",
                schema: "public");

            migrationBuilder.DropTable(
                name: "StoredConfigs",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Users",
                schema: "public");

            migrationBuilder.DropTable(
                name: "Contests",
                schema: "public");
        }
    }
}
