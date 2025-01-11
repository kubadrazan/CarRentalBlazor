using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MiniCarRentalAPI.Migrations
{
    /// <inheritdoc />
    public partial class DeleteApisTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiEntities");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiEntities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApiID = table.Column<int>(type: "int", nullable: false),
                    ApiKeyHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ApiKeySalt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    URL = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiEntities", x => x.ID);
                });
        }
    }
}
