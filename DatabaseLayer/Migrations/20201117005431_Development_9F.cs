using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_9F : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "931457aa-cd90-49c1-88e3-75989f71899b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fbf39027-4f54-4efc-96c0-fc39f11914be");

            migrationBuilder.AddColumn<string>(
                name: "NameString",
                table: "UserArticles",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "39a968c5-2c1a-477a-96fa-24fdece2e9b2", "1fed1d8a-53f7-457c-8013-875fcb7b8165", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1b54b870-5a4d-4ab5-8452-3a9e99559ba6", "d268ece2-7a06-43b2-86a9-2701f4e4fafd", "manager", "MANAGER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1b54b870-5a4d-4ab5-8452-3a9e99559ba6");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "39a968c5-2c1a-477a-96fa-24fdece2e9b2");

            migrationBuilder.DropColumn(
                name: "NameString",
                table: "UserArticles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fbf39027-4f54-4efc-96c0-fc39f11914be", "86d11b8c-d8da-4fa0-bdce-84a905488097", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "931457aa-cd90-49c1-88e3-75989f71899b", "1fb1ec26-744c-40df-adcb-b76f44509d0d", "manager", "MANAGER" });
        }
    }
}
