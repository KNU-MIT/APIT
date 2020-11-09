using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_7b : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "8db5c8fd-53b9-4ae5-a7ad-e62bfb269088", "9ab665b2-804d-4656-bf67-78f05d1da3db", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aceb5a31-8a76-4aca-b6eb-48a3089d710f", "157ddbc7-0ef1-4c68-9566-5c3711735e57", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8db5c8fd-53b9-4ae5-a7ad-e62bfb269088");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "aceb5a31-8a76-4aca-b6eb-48a3089d710f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d23a4613-6d7d-42c4-aa9d-a75f6831a97d", "6f7142a7-7bcc-4e2c-9416-111a606b117e", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "35694013-b775-45ec-ad37-3bf0f5e4d049", "581ceaca-2b33-40a2-9b64-30ece0cf049c", "organizer", "ORGANIZER" });
        }
    }
}
