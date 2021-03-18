using Microsoft.EntityFrameworkCore.Migrations;

namespace Group1.Data.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ba6c021f-02fa-46da-ab5b-bcf7e71c5ff4");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "Street", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ac20b398-c785-4bf7-b1f2-a25b94956895", 0, null, "790d2e40-35c3-4b94-b5ce-0a7e679788e9", "admin@admin.com", true, null, true, null, false, null, "admin@admin.com", "admin@admin.com", "AQAAAAEAACcQAAAAEOfrzICR+a2kPeKEK+mWzlpHnJgBw7lbVr78zoq0tvImels71oWIyOuK+pYtjIMjUg==", null, false, null, "", null, false, "admin@admin.com" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac20b398-c785-4bf7-b1f2-a25b94956895");

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "City", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "IsAdmin", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "PostalCode", "SecurityStamp", "Street", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ba6c021f-02fa-46da-ab5b-bcf7e71c5ff4", 0, null, "8e259e0b-a622-4e79-8fb0-f11609afdfd8", "admin@admin.com", true, null, true, null, false, null, "admin@admin.com", "admin@admin.com", "AQAAAAEAACcQAAAAEDcLGIWNdYE3UnzG3QYOq1GTkrpXj1+LV7eBeT7IaQJq6X9XLWTjkvhHLwaGnGI1zA==", null, false, null, "", null, false, "admin@admin.com" });
        }
    }
}
