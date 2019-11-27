using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanGiayASP.Data.Migrations
{
    public partial class addImportDetailsAndStores : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductStores");

            migrationBuilder.DropColumn(
                name: "Address",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "ImportDetailID",
                table: "Stores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProductID",
                table: "Stores",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ImportDetails",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateImport = table.Column<DateTime>(nullable: false),
                    Total = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ImportDetails", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ImportDetailID",
                table: "Stores",
                column: "ImportDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ProductID",
                table: "Stores",
                column: "ProductID");

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_ImportDetails_ImportDetailID",
                table: "Stores",
                column: "ImportDetailID",
                principalTable: "ImportDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stores_Products_ProductID",
                table: "Stores",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stores_ImportDetails_ImportDetailID",
                table: "Stores");

            migrationBuilder.DropForeignKey(
                name: "FK_Stores_Products_ProductID",
                table: "Stores");

            migrationBuilder.DropTable(
                name: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ImportDetailID",
                table: "Stores");

            migrationBuilder.DropIndex(
                name: "IX_Stores_ProductID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ImportDetailID",
                table: "Stores");

            migrationBuilder.DropColumn(
                name: "ProductID",
                table: "Stores");

            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Stores",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProductStores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    StoreID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ProductStores_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductStores_Stores_StoreID",
                        column: x => x.StoreID,
                        principalTable: "Stores",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductStores_ProductID",
                table: "ProductStores",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductStores_StoreID",
                table: "ProductStores",
                column: "StoreID");
        }
    }
}
