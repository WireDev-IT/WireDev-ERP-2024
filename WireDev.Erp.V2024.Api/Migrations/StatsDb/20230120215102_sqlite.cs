using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.StatsDb
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DayStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayStats", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "MonthStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthStats", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "ProductStats",
                columns: table => new
                {
                    ProductId = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Transactions = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductStats", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "TotalStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalStats", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "YearStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItemSells = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearStats", x => x.Date);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayStats");

            migrationBuilder.DropTable(
                name: "MonthStats");

            migrationBuilder.DropTable(
                name: "ProductStats");

            migrationBuilder.DropTable(
                name: "TotalStats");

            migrationBuilder.DropTable(
                name: "YearStats");
        }
    }
}
