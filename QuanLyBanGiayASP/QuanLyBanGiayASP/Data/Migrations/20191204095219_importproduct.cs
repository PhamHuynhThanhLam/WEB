using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanGiayASP.Data.Migrations
{
    public partial class importproduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "ImportDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ImportDetails_ProductId",
                table: "ImportDetails",
                column: "ProductId");

            migrationBuilder.AddForeignKey(
                name: "FK_ImportDetails_Products_ProductId",
                table: "ImportDetails",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ImportDetails_Products_ProductId",
                table: "ImportDetails");

            migrationBuilder.DropIndex(
                name: "IX_ImportDetails_ProductId",
                table: "ImportDetails");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "ImportDetails");
        }
    }
}
