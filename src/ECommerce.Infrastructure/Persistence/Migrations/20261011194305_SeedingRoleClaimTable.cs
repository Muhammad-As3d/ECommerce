using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeedingRoleClaimTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "permissions", "product:read", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 2, "permissions", "product:update", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 3, "permissions", "product:delete", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 4, "permissions", "cart:read", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 5, "permissions", "cart:add", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 6, "permissions", "cart:remove", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 7, "permissions", "cart:update", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 8, "permissions", "cart:clear", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 9, "permissions", "order:read", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 10, "permissions", "order:read all", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 11, "permissions", "order:read user", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 12, "permissions", "order:checkout", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 13, "permissions", "order:cancel", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 14, "permissions", "order:process", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 15, "permissions", "order:ship", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 16, "permissions", "order:deliver", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 17, "permissions", "order:status-history", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 18, "permissions", "order:update", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 19, "permissions", "order:delete", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 20, "permissions", "category:read", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 21, "permissions", "category:read all", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 22, "permissions", "category:read products", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 23, "permissions", "category:update", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 24, "permissions", "category:add", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 25, "permissions", "category:toggle", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 26, "permissions", "product:read", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 27, "permissions", "product:add", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 28, "permissions", "product:update", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 29, "permissions", "product:toggle", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 30, "permissions", "product:add images", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" },
                    { 31, "permissions", "product:remove images", "b0d60c5c-4d20-4991-9171-772a0a8bd2f8" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 26);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 28);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 29);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 30);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 31);
        }
    }
}
