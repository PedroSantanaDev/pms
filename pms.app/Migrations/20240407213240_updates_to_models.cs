using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pms.app.Migrations
{
    /// <inheritdoc />
    public partial class updates_to_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerItems_Customers_CustomerId",
                table: "CustomerItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerItems_Items_ItemId",
                table: "CustomerItems");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "CustomerItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CustomerItems",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerItems_Customers_CustomerId",
                table: "CustomerItems",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerItems_Items_ItemId",
                table: "CustomerItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CustomerItems_Customers_CustomerId",
                table: "CustomerItems");

            migrationBuilder.DropForeignKey(
                name: "FK_CustomerItems_Items_ItemId",
                table: "CustomerItems");

            migrationBuilder.AlterColumn<int>(
                name: "ItemId",
                table: "CustomerItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CustomerId",
                table: "CustomerItems",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerItems_Customers_CustomerId",
                table: "CustomerItems",
                column: "CustomerId",
                principalTable: "Customers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CustomerItems_Items_ItemId",
                table: "CustomerItems",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
