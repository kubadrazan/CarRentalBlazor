using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiNICarRentalBrowser.Migrations
{
    /// <inheritdoc />
    public partial class USerChangedLocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Localizations_LocalizationID",
                table: "Users");

            migrationBuilder.DropTable(
                name: "Localizations");

            migrationBuilder.DropIndex(
                name: "IX_Users_LocalizationID",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "LocalizationID",
                table: "Users");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Location_Latitude",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Location_Longitude",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "LocalizationID",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Localizations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HouseNumber = table.Column<int>(type: "int", nullable: false),
                    Street = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Localizations", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Users_LocalizationID",
                table: "Users",
                column: "LocalizationID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Localizations_LocalizationID",
                table: "Users",
                column: "LocalizationID",
                principalTable: "Localizations",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
