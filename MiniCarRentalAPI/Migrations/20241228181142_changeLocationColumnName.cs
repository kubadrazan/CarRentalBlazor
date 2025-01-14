using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class changeLocationColumnName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Returns",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Returns",
                newName: "Location_Latitude");

            migrationBuilder.RenameColumn(
                name: "Longitude",
                table: "Cars",
                newName: "Location_Longitude");

            migrationBuilder.RenameColumn(
                name: "Latitude",
                table: "Cars",
                newName: "Location_Latitude");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Returns",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Returns",
                newName: "Latitude");

            migrationBuilder.RenameColumn(
                name: "Location_Longitude",
                table: "Cars",
                newName: "Longitude");

            migrationBuilder.RenameColumn(
                name: "Location_Latitude",
                table: "Cars",
                newName: "Latitude");
        }
    }
}
