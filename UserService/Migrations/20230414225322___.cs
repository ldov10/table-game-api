using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace UserService.Migrations
{
    public partial class __ : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Indentifier",
                table: "Users",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "Indentifier",
                table: "RefreshTokens",
                newName: "Identifier");

            migrationBuilder.RenameColumn(
                name: "Indentifier",
                table: "Images",
                newName: "Identifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDtm",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDtm",
                table: "RefreshTokens");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                table: "Users",
                newName: "Indentifier");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                table: "RefreshTokens",
                newName: "Indentifier");

            migrationBuilder.RenameColumn(
                name: "Identifier",
                table: "Images",
                newName: "Indentifier");
        }
    }
}
