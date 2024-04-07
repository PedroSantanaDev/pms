using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pms.app.Migrations
{
    /// <inheritdoc />
    public partial class adding_fields_to_customer_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "AssignedDate",
                table: "CustomerItems",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "CustomerItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AssignedDate",
                table: "CustomerItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "CustomerItems");
        }
    }
}
