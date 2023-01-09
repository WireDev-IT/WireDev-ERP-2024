using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.PriceDb
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

            migrationBuilder.InsertData(
                table: "Prices",
                columns: new[] { "Uuid", "Archived", "Description", "Locked", "RetailValue", "SellValue" },
                values: new object[] { new Guid("ad468c84-4215-44df-a765-da137ddcb1b6"), false, "Default_Price", false, 0m, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Prices");
        }
    }
}
