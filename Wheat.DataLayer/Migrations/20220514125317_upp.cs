using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class upp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "SellContracts",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "SellContracts",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_Code",
                table: "AspNetUsers",
                column: "Code",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_Code",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "SellContracts");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "SellerId",
                table: "SellContracts",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SellContracts_AspNetUsers_SellerId",
                table: "SellContracts",
                column: "SellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
