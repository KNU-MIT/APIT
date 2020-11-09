using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_7c : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                values: new object[] { "e0e8c82a-abce-4fe9-8e5b-82bbd2c4b1ef", "d1a14b5c-5415-47d2-bd4a-7c2896a12067", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ee4d8e14-1722-4607-b187-1ef7a49b6281", "0ad09c88-0414-4987-844f-fdd901271724", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e0e8c82a-abce-4fe9-8e5b-82bbd2c4b1ef");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ee4d8e14-1722-4607-b187-1ef7a49b6281");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8db5c8fd-53b9-4ae5-a7ad-e62bfb269088", "9ab665b2-804d-4656-bf67-78f05d1da3db", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "aceb5a31-8a76-4aca-b6eb-48a3089d710f", "157ddbc7-0ef1-4c68-9566-5c3711735e57", "manager", "MANAGER" });
        }
    }
}
