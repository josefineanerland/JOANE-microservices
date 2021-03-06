using Microsoft.EntityFrameworkCore.Migrations;

namespace Group1.Data.Migrations
{
    public partial class AddAdmin_isadmin : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "d25316c5-e4c8-465a-9cd8-cb3392b07f0f");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "Street", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ba6c021f-02fa-46da-ab5b-bcf7e71c5ff4", 0, null, "8e259e0b-a622-4e79-8fb0-f11609afdfd8", "admin@admin.com", true, null, true, null, false, null, "admin@admin.com", "admin@admin.com", "AQAAAAEAACcQAAAAEDcLGIWNdYE3UnzG3QYOq1GTkrpXj1+LV7eBeT7IaQJq6X9XLWTjkvhHLwaGnGI1zA==", null, false, null, "", null, false, "admin@admin.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ba6c021f-02fa-46da-ab5b-bcf7e71c5ff4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "Street", "TwoFactorEnabled", "UserName" },
                values: new object[] { "d25316c5-e4c8-465a-9cd8-cb3392b07f0f", 0, null, "55babb5b-fb6f-4b69-bd1f-f7046a21782c", "admin@admin.com", true, null, false, null, false, null, "admin@admin.com", "admin@admin.com", "AQAAAAEAACcQAAAAEK8rlWpiT4Cqz3izzyoasgXIAH/3SAaiSN1bOPG6vlpGz0/Q4XUWWuxdkG7IawPwKg==", null, false, null, "", null, false, "admin@admin.com" });
        }
    }
}
