using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class ChagneEmployeeIDToEmailInAcceptationTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeID",
                table: "Acceptations");

            migrationBuilder.AddColumn<string>(
                name: "EmployeeEmail",
                table: "Acceptations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmployeeEmail",
                table: "Acceptations");

            migrationBuilder.AddColumn<int>(
                name: "EmployeeID",
                table: "Acceptations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
