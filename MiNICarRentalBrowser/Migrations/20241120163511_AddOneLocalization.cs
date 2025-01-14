using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiNICarRentalBrowser.Migrations
{
    /// <inheritdoc />
    public partial class AddOneLocalization : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("\r\nSET IDENTITY_INSERT Localizations ON;INSERT INTO Localizations (ID, Country, City, Street, HouseNumber)\n" +
                "VALUES(0, 'Poland', 'Warsaw', 'Koszykowa', 75); \r\nSET IDENTITY_INSERT Localizations OFF");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("delete from Localizations");
        }
    }
}
