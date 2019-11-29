using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanGiayASP.Data.Migrations
{
    public partial class @new : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.AddColumn<int>(
                name: "ImportDetailID",
                table: "Products",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Products_ImportDetailID",
                table: "Products",
                column: "ImportDetailID");

            migrationBuilder.AddForeignKey(
                name: "FK_Products_ImportDetails_ImportDetailID",
                table: "Products",
                column: "ImportDetailID",
                principalTable: "ImportDetails",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Products_ImportDetails_ImportDetailID",
                table: "Products");

            migrationBuilder.DropIndex(
                name: "IX_Products_ImportDetailID",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "ImportDetailID",
                table: "Products");

            migrationBuilder.CreateTable(
                name: "Stores",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ImportDetailID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stores", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Stores_ImportDetails_ImportDetailID",
                        column: x => x.ImportDetailID,
                        principalTable: "ImportDetails",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stores_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ImportDetailID",
                table: "Stores",
                column: "ImportDetailID");

            migrationBuilder.CreateIndex(
                name: "IX_Stores_ProductID",
                table: "Stores",
                column: "ProductID");
        }
    }
}
