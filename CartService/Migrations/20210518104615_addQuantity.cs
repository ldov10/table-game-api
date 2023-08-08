using Microsoft.EntityFrameworkCore.Migrations;

namespace CartService.Migrations
{
    public partial class addQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets");

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Buckets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Buckets");

            migrationBuilder.AddForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
