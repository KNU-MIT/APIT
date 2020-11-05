using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_5t : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "680c4561-2fe7-4338-8c32-71db945d7016");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d415b6db-4d3e-4b85-affa-76fec676554f");

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Articles",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e774621d-7409-4151-a8dd-dbd5e50d8d9d", "ea3a71dd-bb0c-4f19-935b-4720ebe5d0a7", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21994968-f46f-40de-86df-15c6abac02d1", "814600e8-2565-4579-b69d-cef94c30ddb4", "organizer", "ORGANIZER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21994968-f46f-40de-86df-15c6abac02d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e774621d-7409-4151-a8dd-dbd5e50d8d9d");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Articles");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "d415b6db-4d3e-4b85-affa-76fec676554f", "7047d359-763f-4474-8a88-1c8c5438be77", "admin", "ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "680c4561-2fe7-4338-8c32-71db945d7016", "786984f1-7c77-4436-861c-923cfeb08c73", "organizer", "ORGANIZER" });
        }
    }
}
