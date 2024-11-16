using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class AddLocalizationData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var filePath = AppDomain.CurrentDomain.BaseDirectory +
                "../../../Migrations/CSVData/localizations.csv";
            migrationBuilder.Sql($"BULK INSERT [dbo].[Localizations]\r\n" +
                $"FROM '{filePath}'\r\n" +
                "WITH (FIRSTROW = 2,\r\n\t" +
                "KEEPIDENTITY,\r\n\t" +
                "FIELDTERMINATOR = ',',\r\n\t" +
                "ROWTERMINATOR = '\\n'\r\n\t)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [dbo].[Localizations]");
        }
    }
}
