using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class @null : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaserId",
                table: "SellContracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts",
                column: "PurchaserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts");

            migrationBuilder.AlterColumn<string>(
                name: "PurchaserId",
                table: "SellContracts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_PurchaserId",
                table: "SellContracts",
                column: "PurchaserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
