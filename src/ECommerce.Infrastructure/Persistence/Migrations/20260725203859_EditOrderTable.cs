using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CouponId",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ShippingMethodId",
                table: "Orders");

            migrationBuilder.AddColumn<int>(
                name: "WithinDays",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WithinDays",
                table: "Orders");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ShippingMethodId",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }
    }
}
