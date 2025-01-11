using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiNICarRentalBrowser.Migrations
{
    /// <inheritdoc />
    public partial class DeleteLocationFromUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Users");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "Location_Latitude",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "Location_Longitude",
                table: "Users",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
