using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiNICarRentalBrowser.Migrations
{
    /// <inheritdoc />
    public partial class ChengedColumnNamesRentalBrowser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiID",
                table: "Rentals",
                newName: "RentalID");
            migrationBuilder.RenameColumn(
               name: "SourceApiID",
               table: "Rentals",
               newName: "ApiID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RentalID",
                table: "Rentals",
                newName: "ApiID");
            migrationBuilder.RenameColumn(
                name: "ApiID",
                table: "Rentals",
                newName: "SourceApiID");
        }
    }
}
