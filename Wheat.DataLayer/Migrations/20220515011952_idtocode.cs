using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class idtocode : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "SellContracts",
                newName: "SellerCode");

            migrationBuilder.RenameColumn(
                name: "PurchaserId",
                table: "SellContracts",
                newName: "PurchaserCode");

            migrationBuilder.RenameIndex(
                name: "IX_SellContracts_SellerId",
                table: "SellContracts",
                newName: "IX_SellContracts_SellerCode");

            migrationBuilder.RenameIndex(
                name: "IX_SellContracts_PurchaserId",
                table: "SellContracts",
                newName: "IX_SellContracts_PurchaserCode");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserCode",
                table: "SellContracts",
                column: "PurchaserCode",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerCode",
                table: "SellContracts",
                column: "SellerCode",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserCode",
                table: "SellContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerCode",
                table: "SellContracts");

            migrationBuilder.RenameColumn(
                name: "SellerCode",
                table: "SellContracts",
                newName: "SellerId");

            migrationBuilder.RenameColumn(
                name: "PurchaserCode",
                table: "SellContracts",
                newName: "PurchaserId");

            migrationBuilder.RenameIndex(
                name: "IX_SellContracts_SellerCode",
                table: "SellContracts",
                newName: "IX_SellContracts_SellerId");

            migrationBuilder.RenameIndex(
                name: "IX_SellContracts_PurchaserCode",
                table: "SellContracts",
                newName: "IX_SellContracts_PurchaserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts",
                column: "PurchaserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
