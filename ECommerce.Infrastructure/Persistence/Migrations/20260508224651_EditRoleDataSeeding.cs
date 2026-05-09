using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditRoleDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23c617eb-34dd-41ca-b15a-b5630999daaa",
                column: "IsDefault",
                value: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23c617eb-34dd-41ca-b15a-b5630999daaa",
                column: "IsDefault",
                value: false);
        }
    }
}
