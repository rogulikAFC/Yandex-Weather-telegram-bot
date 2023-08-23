using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherAPI.Migrations
{
    /// <inheritdoc />
    public partial class UserTelegramIdRenamedToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TelegramId",
                table: "Users",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_Users_TelegramId",
                table: "Users",
                newName: "IX_Users_Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Users",
                newName: "TelegramId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Id",
                table: "Users",
                newName: "IX_Users_TelegramId");
        }
    }
}
