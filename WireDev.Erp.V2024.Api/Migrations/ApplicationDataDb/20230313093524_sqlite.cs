using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ApplicationDataDb
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Audit",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    AuditDateTimeUtc = table.Column<DateTime>(type: "TEXT", nullable: false),
                    AuditType = table.Column<string>(type: "TEXT", nullable: true),
                    AuditUser = table.Column<string>(type: "TEXT", nullable: true),
                    TableName = table.Column<string>(type: "TEXT", nullable: true),
                    KeyValues = table.Column<string>(type: "TEXT", nullable: true),
                    OldValues = table.Column<string>(type: "TEXT", nullable: true),
                    NewValues = table.Column<string>(type: "TEXT", nullable: true),
                    ChangedColumns = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Audit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DayStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expenses = table.Column<decimal>(type: "TEXT", nullable: false),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    PurchasedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayStats", x => x.Date);
                });

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Uuid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Color = table.Column<string>(type: "TEXT", nullable: true),
                    Used = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "MonthStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expenses = table.Column<decimal>(type: "TEXT", nullable: false),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    PurchasedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthStats", x => x.Date);
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
                    Uuid = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Used = table.Column<bool>(type: "INTEGER", nullable: false),
                    Group = table.Column<int>(type: "INTEGER", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Active = table.Column<bool>(type: "INTEGER", nullable: false),
                    Archived = table.Column<bool>(type: "INTEGER", nullable: false),
                    Availible = table.Column<int>(type: "INTEGER", nullable: false),
                    EAN = table.Column<string>(type: "TEXT", nullable: false),
                    Prices = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Uuid);
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
                name: "Purchases",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(5, 2)", nullable: false),
                    DatePosted = table.Column<DateTime>(type: "TEXT", nullable: true),
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false),
                    Posted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "Role",
                columns: table => new
                {
                    id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    name = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    details = table.Column<string>(type: "TEXT", maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Role", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    Uuid = table.Column<Guid>(type: "TEXT", nullable: false),
                    NextProductNumber = table.Column<uint>(type: "INTEGER", nullable: false),
                    NextGroupNumber = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.Uuid);
                });

            migrationBuilder.CreateTable(
                name: "TotalStats",
                columns: table => new
                {
                    Date = table.Column<long>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Expenses = table.Column<decimal>(type: "TEXT", nullable: false),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    PurchasedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItems = table.Column<uint>(type: "INTEGER", nullable: false),
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
                    Expenses = table.Column<decimal>(type: "TEXT", nullable: false),
                    Revenue = table.Column<decimal>(type: "TEXT", nullable: false),
                    Losses = table.Column<decimal>(type: "TEXT", nullable: false),
                    SoldItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    PurchasedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    CanceledItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    RefundedItems = table.Column<uint>(type: "INTEGER", nullable: false),
                    DisposedItems = table.Column<uint>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YearStats", x => x.Date);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Uuid", "Color", "Description", "Name", "Used" },
                values: new object[] { 99, null, null, "Default_Group", false });

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Uuid", "Archived", "Description", "Locked", "RetailValue", "SellValue" },
                values: new object[] { new Guid("bdd9faca-f05f-4658-a1a7-61c978078a13"), false, "Defaul_Price", false, 10m, 15m });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Uuid", "Active", "Archived", "Availible", "Description", "EAN", "Group", "Metadata", "Name", "Prices", "Properties", "Used" },
                values: new object[] { 9999u, false, false, 0, null, "[]", 100, "{}", "Default_Product", "[\"bdd9faca-f05f-4658-a1a7-61c978078a13\"]", "{}", false });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "Uuid", "DatePosted", "Items", "Posted", "TotalPrice", "Type" },
                values: new object[] { new Guid("0265d7dc-da76-41d1-8c75-43dfb919ae64"), new DateTime(2023, 3, 13, 9, 35, 24, 362, DateTimeKind.Utc).AddTicks(630), "[]", true, 0m, 0 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Uuid", "NextGroupNumber", "NextProductNumber" },
                values: new object[] { new Guid("47a542b6-4856-4e44-9bc4-15bd7d896eed"), 100, 10000u });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Audit");

            migrationBuilder.DropTable(
                name: "DayStats");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "MonthStats");

            migrationBuilder.DropTable(
                name: "Prices");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductStats");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Role");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TotalStats");

            migrationBuilder.DropTable(
                name: "YearStats");
        }
    }
}
