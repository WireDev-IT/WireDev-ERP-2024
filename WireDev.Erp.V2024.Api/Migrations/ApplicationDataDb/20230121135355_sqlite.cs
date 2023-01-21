﻿using System;
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

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Uuid", "Color", "Description", "Name", "Used" },
                values: new object[] { 99, null, null, "Default_Group", false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Uuid", "Active", "Archived", "Availible", "Description", "EAN", "Group", "Metadata", "Name", "Prices", "Properties", "Used" },
                values: new object[] { 9999u, false, false, 0, null, "[]", 100, "{}", "Default_Product", "[{\"Uuid\":\"2b7bf118-f4b0-47b7-96ca-d99925a3d778\",\"Archived\":false,\"Description\":\"Defaul_Price\",\"RetailValue\":10,\"SellValue\":15,\"Locked\":false}]", "{}", false });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "Uuid", "DatePosted", "Items", "Posted", "TotalPrice", "Type" },
                values: new object[] { new Guid("5c21e9c8-4c84-4787-bf12-c066399bfd0a"), new DateTime(2023, 1, 21, 13, 53, 55, 231, DateTimeKind.Utc).AddTicks(5630), "[]", true, 0m, 0 });

            migrationBuilder.InsertData(
                table: "Settings",
                columns: new[] { "Uuid", "NextGroupNumber", "NextProductNumber" },
                values: new object[] { new Guid("9ddda2b6-a24e-4894-bf0b-c0e3b39c5550"), 100, 10000u });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DayStats");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "MonthStats");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ProductStats");

            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TotalStats");

            migrationBuilder.DropTable(
                name: "YearStats");
        }
    }
}
