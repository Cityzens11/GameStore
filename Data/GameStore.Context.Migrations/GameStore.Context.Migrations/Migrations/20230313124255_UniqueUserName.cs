using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.Context.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UniqueUserName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_users_UserName",
                table: "users",
                column: "UserName",
                unique: true,
                filter: "[UserName] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_users_UserName",
                table: "users");
        }
    }
}
