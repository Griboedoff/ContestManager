using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Core.Migrations
{
    public partial class AccountId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationCode",
                schema: "public",
                table: "EmailConfirmationRequests",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 7,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AccountId",
                schema: "public",
                table: "EmailConfirmationRequests",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                schema: "public",
                table: "AuthenticationAccounts",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountId",
                schema: "public",
                table: "EmailConfirmationRequests");

            migrationBuilder.DropColumn(
                name: "IsActive",
                schema: "public",
                table: "AuthenticationAccounts");

            migrationBuilder.AlterColumn<string>(
                name: "ConfirmationCode",
                schema: "public",
                table: "EmailConfirmationRequests",
                maxLength: 7,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }
    }
}
