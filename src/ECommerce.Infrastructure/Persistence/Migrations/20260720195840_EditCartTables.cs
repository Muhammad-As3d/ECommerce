using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditCartTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CartItems_Carts_CartId1",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems");

            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId1",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CartId1",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedOn",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UpdatedById",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "UpdatedOn",
                table: "CartItems");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems",
                columns: new[] { "CartId", "ProductId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CartItems_CartId_ProductId",
                table: "CartItems");

            migrationBuilder.AddColumn<Guid>(
                name: "CartId1",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedOn",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "UpdatedById",
                table: "CartItems",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedOn",
                table: "CartItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId",
                table: "CartItems",
                column: "CartId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_CartId1",
                table: "CartItems",
                column: "CartId1");

            migrationBuilder.AddForeignKey(
                name: "FK_CartItems_Carts_CartId1",
                table: "CartItems",
                column: "CartId1",
                principalTable: "Carts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
