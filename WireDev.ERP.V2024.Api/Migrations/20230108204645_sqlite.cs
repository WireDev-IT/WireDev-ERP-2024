using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Prices",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Archived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    RetailValue = table.Column<decimal>(type: "decimal(5, 3)", nullable: false),
                    SellValue = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    Locked = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prices", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    Used = table.Column<bool>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Archived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Availible = table.Column<uint>(type: "INTEGER", nullable: false),
                    Prices = table.Column<string>(type: "TEXT", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Items = table.Column<string>(type: "TEXT", nullable: false),
                    Posted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Uuid);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Uuid", "Color", "Description", "Name" },
                values: new object[] { new Guid("1c96c230-a083-4a8e-923f-30383ecf136d"), null, "Default_Catetgory", null });

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Uuid", "Archived", "Description", "Locked", "RetailValue", "SellValue" },
                values: new object[] { new Guid("832975c0-8992-4b92-a561-c1e486e39ab5"), false, "Default_Price", false, 0m, 0m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Uuid", "Active", "Archived", "Availible", "Categories", "Description", "Metadata", "Name", "Prices", "Properties", "Used" },
                values: new object[] { new Guid("946bf7d2-3a92-4c8a-8d37-7be2d7f1aed0"), false, false, 0u, "[]", null, "{}", "Default_Product", "[]", "{}", false });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "Uuid", "DatePosted", "Items", "Posted", "TotalPrice" },
                values: new object[] { new Guid("2ce5bea1-93f9-4fd5-90e8-c06a6828be02"), new DateTime(2023, 1, 8, 20, 46, 45, 97, DateTimeKind.Utc).AddTicks(7600), "{}", true, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
