using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ECommerce.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddDataSeeding : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "23c617eb-34dd-41ca-b15a-b5630999daaa", "ffc2d9b2-b2f9-4bcc-af93-eec61e521c87", false, false, "Customer", "CUSTOMER" },
                    { "b0d60c5c-4d20-4991-9171-772a0a8bd2f8", "aaa623d1-2a70-49e8-96e3-53bd3380149a", false, false, "Admin", "ADMIN" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsDisabled", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "D7817610-68FA-4707-88A9-1CED640C8BF9", 0, "A1B2C3D4E5F67890ABCDEF1234567890", "admin@surveybasket.com", true, "Muhammad", false, "Asaad", false, null, "ADMIN@SURVEYBASKET.COM", "ADMIN@SURVEYBASKET.COM", "AQAAAAIAAYagAAAAEFySCHpGeKeygItao5hRquACwjYkd1vfkwg6sHzojSPqiO8Z2iM9dN6qVQuog6hrtA==", null, false, "A1B2C3D8-E5F6-7892-ABCD-EF1234567890", false, "admin@surveybasket.com" });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "b0d60c5c-4d20-4991-9171-772a0a8bd2f8", "D7817610-68FA-4707-88A9-1CED640C8BF9" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "23c617eb-34dd-41ca-b15a-b5630999daaa");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "b0d60c5c-4d20-4991-9171-772a0a8bd2f8", "D7817610-68FA-4707-88A9-1CED640C8BF9" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b0d60c5c-4d20-4991-9171-772a0a8bd2f8");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "D7817610-68FA-4707-88A9-1CED640C8BF9");
        }
    }
}
