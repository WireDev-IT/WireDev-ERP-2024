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
                    Type = table.Column<int>(type: "INTEGER", nullable: false),
                    Items = table.Column<string>(type: "TEXT", nullable: false),
                    Posted = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.Uuid);
                });

            migrationBuilder.InsertData(
                table: "Purchases",
                columns: new[] { "Uuid", "DatePosted", "Items", "Posted", "TotalPrice", "Type" },
                values: new object[] { new Guid("4dc6dae8-601b-430d-9d35-f966333f4adc"), new DateTime(2023, 1, 20, 21, 47, 20, 76, DateTimeKind.Utc).AddTicks(2480), "[]", true, 0m, 0 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");
        }
    }
}
