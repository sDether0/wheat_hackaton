using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class nullls : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationResults_AspNetUsers_BuyerId",
                table: "OperationResults");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "OperationResults",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddForeignKey(
                name: "FK_OperationResults_AspNetUsers_BuyerId",
                table: "OperationResults",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OperationResults_AspNetUsers_BuyerId",
                table: "OperationResults");

            migrationBuilder.AlterColumn<string>(
                name: "BuyerId",
                table: "OperationResults",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OperationResults_AspNetUsers_BuyerId",
                table: "OperationResults",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
