using Microsoft.EntityFrameworkCore.Migrations;

namespace CartService.Migrations
{
    public partial class UserIdToUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Buckets");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Buckets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Username",
                table: "Buckets");

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Buckets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }
    }
}
