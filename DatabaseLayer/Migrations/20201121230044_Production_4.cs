using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Production_4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27572a1e-3628-4d44-be7f-76a417449563");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9d819d0-2d6b-4f79-b618-78c85b6c911b");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "351b7c72-9633-4256-b316-3d5535d8cc68", "e1628b9b-5937-4f98-8a65-19819d96891e", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f43b3a48-9118-452f-b691-4383c2e862c6", "1d5e29f2-750e-4ce1-a2e0-e07e07d836fc", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "351b7c72-9633-4256-b316-3d5535d8cc68");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f43b3a48-9118-452f-b691-4383c2e862c6");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f9d819d0-2d6b-4f79-b618-78c85b6c911b", "75c54b89-6e32-4c12-8ad2-da631610f73a", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "27572a1e-3628-4d44-be7f-76a417449563", "2604934e-375c-4ef0-8d43-26fb292e52ec", "manager", "MANAGER" });
        }
    }
}
