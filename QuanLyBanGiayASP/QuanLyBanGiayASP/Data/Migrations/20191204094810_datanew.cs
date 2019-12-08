using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanGiayASP.Data.Migrations
{
    public partial class datanew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stores");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "ImportDetails");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Total",
                table: "ImportDetails",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
