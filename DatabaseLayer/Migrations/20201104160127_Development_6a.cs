using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_6a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21994968-f46f-40de-86df-15c6abac02d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e774621d-7409-4151-a8dd-dbd5e50d8d9d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d23a4613-6d7d-42c4-aa9d-a75f6831a97d", "6f7142a7-7bcc-4e2c-9416-111a606b117e", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35694013-b775-45ec-ad37-3bf0f5e4d049", "581ceaca-2b33-40a2-9b64-30ece0cf049c", "organizer", "ORGANIZER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "35694013-b775-45ec-ad37-3bf0f5e4d049");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d23a4613-6d7d-42c4-aa9d-a75f6831a97d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e774621d-7409-4151-a8dd-dbd5e50d8d9d", "ea3a71dd-bb0c-4f19-935b-4720ebe5d0a7", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21994968-f46f-40de-86df-15c6abac02d1", "814600e8-2565-4579-b69d-cef94c30ddb4", "organizer", "ORGANIZER" });
        }
    }
}
