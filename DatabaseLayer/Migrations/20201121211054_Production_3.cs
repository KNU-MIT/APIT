using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Production_3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "162d9584-0ead-4e4f-a472-07cfb75216ac");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "26a96ec8-1099-4a55-8517-c436caae6831");

            migrationBuilder.AddColumn<bool>(
                name: "IsRegisteredUser",
                table: "UserArticles",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f9d819d0-2d6b-4f79-b618-78c85b6c911b", "75c54b89-6e32-4c12-8ad2-da631610f73a", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "27572a1e-3628-4d44-be7f-76a417449563", "2604934e-375c-4ef0-8d43-26fb292e52ec", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27572a1e-3628-4d44-be7f-76a417449563");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f9d819d0-2d6b-4f79-b618-78c85b6c911b");

            migrationBuilder.DropColumn(
                name: "IsRegisteredUser",
                table: "UserArticles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "26a96ec8-1099-4a55-8517-c436caae6831", "0539ceba-47a0-44a1-8665-01a5ee267a04", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "162d9584-0ead-4e4f-a472-07cfb75216ac", "0444e043-f841-4f81-8b1e-aba902528509", "manager", "MANAGER" });
        }
    }
}
