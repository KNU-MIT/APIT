using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_9H : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ac0d5392-0259-468b-b17b-f06b33cd2e93");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "b3e3bba4-ba47-4b62-8205-143e2a58a67d");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserArticles",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles",
                column: "Id");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "1fdb9724-e4dc-42a7-ac07-f2516e417ddd", "6b574a1f-056f-4157-9cef-e88d74659030", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f1e64b51-a40c-4313-aca0-4c5ee10fa755", "374dcf8d-bd67-468e-9096-24ff1c1e88e7", "manager", "MANAGER" });

            migrationBuilder.CreateIndex(
                name: "IX_UserArticles_UserId",
                table: "UserArticles",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles");

            migrationBuilder.DropIndex(
                name: "IX_UserArticles_UserId",
                table: "UserArticles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1fdb9724-e4dc-42a7-ac07-f2516e417ddd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1e64b51-a40c-4313-aca0-4c5ee10fa755");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "UserArticles",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "ac0d5392-0259-468b-b17b-f06b33cd2e93", "22dc329d-8fb0-489c-b6e2-cbbcf452abad", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "b3e3bba4-ba47-4b62-8205-143e2a58a67d", "a4c8d24a-bcff-46e3-b242-16812447b8e2", "manager", "MANAGER" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
