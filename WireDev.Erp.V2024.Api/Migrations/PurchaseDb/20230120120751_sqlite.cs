using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.PurchaseDb
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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
                table: "Purchases",
                columns: new[] { "Uuid", "DatePosted", "Items", "Posted", "TotalPrice" },
                values: new object[] { new Guid("21b2c524-3ecf-457a-bbd8-4389d7131694"), new DateTime(2023, 1, 20, 12, 7, 50, 996, DateTimeKind.Utc).AddTicks(900), "[]", true, 0m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
