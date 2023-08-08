using Microsoft.EntityFrameworkCore.Migrations;

namespace CatalogService.Migrations
{
    public partial class _2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Players",
                table: "Products",
                newName: "MinPlayers");

            migrationBuilder.AddColumn<int>(
                name: "MaxPlayers",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MaxPlayers",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "MinPlayers",
                table: "Products",
                newName: "Players");
        }
    }
}
