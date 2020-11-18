using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_9G : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b54b870-5a4d-4ab5-8452-3a9e99559ba6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39a968c5-2c1a-477a-96fa-24fdece2e9b2");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ac0d5392-0259-468b-b17b-f06b33cd2e93", "22dc329d-8fb0-489c-b6e2-cbbcf452abad", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3e3bba4-ba47-4b62-8205-143e2a58a67d", "a4c8d24a-bcff-46e3-b242-16812447b8e2", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac0d5392-0259-468b-b17b-f06b33cd2e93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3e3bba4-ba47-4b62-8205-143e2a58a67d");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "39a968c5-2c1a-477a-96fa-24fdece2e9b2", "1fed1d8a-53f7-457c-8013-875fcb7b8165", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b54b870-5a4d-4ab5-8452-3a9e99559ba6", "d268ece2-7a06-43b2-86a9-2701f4e4fafd", "manager", "MANAGER" });
        }
    }
}
