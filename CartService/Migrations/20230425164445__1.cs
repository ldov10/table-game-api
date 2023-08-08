using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CartService.Migrations
{
    public partial class _1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Buckets_ProductId",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "Buckets");

            migrationBuilder.AddColumn<Guid>(
                name: "Identifier",
                table: "Buckets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProductIdentifier",
                table: "Buckets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserIdentifier",
                table: "Buckets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ActiveProducts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductIdentifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Identifier = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ActiveProducts", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ActiveProducts");

            migrationBuilder.DropColumn(
                name: "Identifier",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "ProductIdentifier",
                table: "Buckets");

            migrationBuilder.DropColumn(
                name: "UserIdentifier",
                table: "Buckets");

            migrationBuilder.AddColumn<long>(
                name: "ProductId",
                table: "Buckets",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "Buckets",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SourceId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Buckets_ProductId",
                table: "Buckets",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_Buckets_Products_ProductId",
                table: "Buckets",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
