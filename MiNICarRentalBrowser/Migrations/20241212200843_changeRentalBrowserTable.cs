using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiNICarRentalBrowser.Migrations
{
    /// <inheritdoc />
    public partial class changeRentalBrowserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RentDate",
                table: "Rentals");

            migrationBuilder.RenameColumn(
                name: "CarID",
                table: "Rentals",
                newName: "ApiID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApiID",
                table: "Rentals",
                newName: "CarID");

            migrationBuilder.AddColumn<DateTime>(
                name: "RentDate",
                table: "Rentals",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
