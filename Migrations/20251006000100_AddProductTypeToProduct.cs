using Microsoft.EntityFrameworkCore.Migrations;

namespace Delivery.Migrations
{
    public partial class AddProductTypeToProduct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProductType",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Normal");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProductType",
                table: "Products");
        }
    }
}
