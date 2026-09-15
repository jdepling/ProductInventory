using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ProductInventory.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValue: new DateTime(2026, 9, 11, 10, 31, 43, 999, DateTimeKind.Local).AddTicks(9437)),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CreatedDate", "Description", "ModifiedDate", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 9, 11, 10, 31, 44, 0, DateTimeKind.Local).AddTicks(149), "High-performance laptop", null, "Laptop", 999.99m, 5 },
                    { 2, new DateTime(2026, 9, 11, 10, 31, 44, 0, DateTimeKind.Local).AddTicks(152), "Wireless mouse", null, "Mouse", 29.99m, 50 },
                    { 3, new DateTime(2026, 9, 11, 10, 31, 44, 0, DateTimeKind.Local).AddTicks(156), "Mechanical keyboard", null, "Keyboard", 89.99m, 20 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
