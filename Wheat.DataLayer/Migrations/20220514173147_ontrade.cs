using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class ontrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "OnTrade",
                table: "SellContracts",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OnTrade",
                table: "SellContracts");
        }
    }
}
