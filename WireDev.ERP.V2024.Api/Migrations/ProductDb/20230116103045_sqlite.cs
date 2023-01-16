using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WireDev.Erp.V1.Api.Migrations.ProductDb
{
    /// <inheritdoc />
    public partial class sqlite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Uuid = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    Used = table.Column<bool>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Uuid);
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
                    Availible = table.Column<uint>(type: "INTEGER", nullable: false),
                    EAN = table.Column<string>(type: "TEXT", nullable: false),
                    Prices = table.Column<string>(type: "TEXT", nullable: false),
                    Categories = table.Column<string>(type: "TEXT", nullable: false),
                    Properties = table.Column<string>(type: "TEXT", nullable: false),
                    Metadata = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Uuid);
                });

            migrationBuilder.InsertData(
                table: "Groups",
                columns: new[] { "Uuid", "Description", "Name", "Used" },
                values: new object[] { 99, null, "Default_Group", false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Uuid", "Active", "Archived", "Availible", "Categories", "Description", "EAN", "Group", "Metadata", "Name", "Prices", "Properties", "Used" },
                values: new object[] { 9999u, false, false, 0u, "[]", null, "[]", 100, "{}", "Default_Product", "[]", "{}", false });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
