using Microsoft.EntityFrameworkCore.Migrations;

namespace GrabAPI.Migrations
{
    public partial class ColumnChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Items_ServiceId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ServiceId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "ServiceId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "AType1",
                table: "AccountType");

            migrationBuilder.AddColumn<string>(
                name: "AcoountType",
                table: "AccountType",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ItemId",
                table: "OrderDetails",
                column: "ItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                table: "OrderDetails",
                column: "ItemId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetails_Items_ItemId",
                table: "OrderDetails");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetails_ItemId",
                table: "OrderDetails");

            migrationBuilder.DropColumn(
                name: "AcoountType",
                table: "AccountType");

            migrationBuilder.AddColumn<int>(
                name: "ServiceId",
                table: "OrderDetails",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AType1",
                table: "AccountType",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ServiceId",
                table: "OrderDetails",
                column: "ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetails_Items_ServiceId",
                table: "OrderDetails",
                column: "ServiceId",
                principalTable: "Items",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
