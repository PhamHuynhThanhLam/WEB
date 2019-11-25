using Microsoft.EntityFrameworkCore.Migrations;

namespace QuanLyBanGiayASP.Data.Migrations
{
    public partial class addOrdersToDatabaseNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_PersonId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_PersonId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "SalesPersonId",
                table: "Orders",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_SalesPersonId",
                table: "Orders",
                column: "SalesPersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_SalesPersonId",
                table: "Orders",
                column: "SalesPersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_SalesPersonId",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_SalesPersonId",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "SalesPersonId",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PersonId",
                table: "Orders",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_PersonId",
                table: "Orders",
                column: "PersonId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_PersonId",
                table: "Orders",
                column: "PersonId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
