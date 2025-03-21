using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TestWebApplication.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyItems",
                columns: table => new
                {
                    CompanyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    StockTicker = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Exchange = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Isin = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    WebUrl = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyItems", x => x.CompanyID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyItems_Isin",
                table: "CompanyItems",
                column: "Isin",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyItems");
        }
    }
}
