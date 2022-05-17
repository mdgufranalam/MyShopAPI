using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopAPI.DataAccess.Migrations
{
    public partial class removeDiscount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiscountPerc",
                table: "Products");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DiscountPerc",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
