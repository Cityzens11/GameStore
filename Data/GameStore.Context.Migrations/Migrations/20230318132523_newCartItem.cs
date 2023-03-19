using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class newCartItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "cartitems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "cartitems");
        }
    }
}
