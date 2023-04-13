using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class idtocode2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserCode",
                table: "SellContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerCode",
                table: "SellContracts");

            migrationBuilder.DropIndex(
                name: "IX_SellContracts_PurchaserCode",
                table: "SellContracts");

            migrationBuilder.DropIndex(
                name: "IX_SellContracts_SellerCode",
                table: "SellContracts");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaserCode",
                table: "SellContracts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BuyerCode",
                table: "SellContracts",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SellerCode1",
                table: "SellContracts",
                type: "text",
                nullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AspNetUsers_Code",
                table: "AspNetUsers",
                column: "Code");

            migrationBuilder.CreateIndex(
                name: "IX_SellContracts_BuyerCode",
                table: "SellContracts",
                column: "BuyerCode");

            migrationBuilder.CreateIndex(
                name: "IX_SellContracts_SellerCode1",
                table: "SellContracts",
                column: "SellerCode1");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_BuyerCode",
                table: "SellContracts",
                column: "BuyerCode",
                principalTable: "AspNetUsers",
                principalColumn: "Code");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerCode1",
                table: "SellContracts",
                column: "SellerCode1",
                principalTable: "AspNetUsers",
                principalColumn: "Code");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_BuyerCode",
                table: "SellContracts");

            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerCode1",
                table: "SellContracts");

            migrationBuilder.DropIndex(
                name: "IX_SellContracts_BuyerCode",
                table: "SellContracts");

            migrationBuilder.DropIndex(
                name: "IX_SellContracts_SellerCode1",
                table: "SellContracts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AspNetUsers_Code",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BuyerCode",
                table: "SellContracts");

            migrationBuilder.DropColumn(
                name: "SellerCode1",
                table: "SellContracts");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaserCode",
                table: "SellContracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_SellContracts_PurchaserCode",
                table: "SellContracts",
                column: "PurchaserCode");

            migrationBuilder.CreateIndex(
                name: "IX_SellContracts_SellerCode",
                table: "SellContracts",
                column: "SellerCode");

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
    }
}
