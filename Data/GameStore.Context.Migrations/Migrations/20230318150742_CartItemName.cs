using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class CartItemName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "cartitems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "cartitems");
        }
    }
}
