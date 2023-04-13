using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheat.DataLayer.Migrations
{
    public partial class oldprice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "OldPrice",
                table: "SellContracts",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OldPrice",
                table: "SellContracts");
        }
    }
}
