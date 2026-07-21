using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EditAdminEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "D7817610-68FA-4707-88A9-1CED640C8BF9",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "admin@ECommerce.com", "ADMIN@ECOMMERCE.COM", "ADMIN@ECOMMERCE.COM", "admin@ECommerce.com" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "D7817610-68FA-4707-88A9-1CED640C8BF9",
                columns: new[] { "Email", "NormalizedEmail", "NormalizedUserName", "UserName" },
                values: new object[] { "admin@surveybasket.com", "ADMIN@SURVEYBASKET.COM", "ADMIN@SURVEYBASKET.COM", "admin@surveybasket.com" });
        }
    }
}
