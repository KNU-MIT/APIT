using Microsoft.EntityFrameworkCore.Migrations;

namespace DatabaseLayer.Migrations
{
    public partial class Development_8a : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserOwnArticlesLinking_Articles_ArticleId",
                table: "UserOwnArticlesLinking");

            migrationBuilder.DropForeignKey(
                name: "FK_UserOwnArticlesLinking_AspNetUsers_UserId",
                table: "UserOwnArticlesLinking");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserOwnArticlesLinking",
                table: "UserOwnArticlesLinking");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "04ca4593-a2c9-4d7c-a7b7-d42bb04a47b8");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fd724de2-7465-49eb-bbd4-8599d388711f");

            migrationBuilder.RenameTable(
                name: "UserOwnArticlesLinking",
                newName: "UserArticles");

            migrationBuilder.RenameIndex(
                name: "IX_UserOwnArticlesLinking_ArticleId",
                table: "UserArticles",
                newName: "IX_UserArticles_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "a2ea3173-fb17-4fc2-8c6e-f3d5791cfdd5", "36988f64-80f1-4684-8b95-f5f668fc3666", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "64fa95a9-a846-499b-a0e1-d302f1b03a86", "53dc403a-9356-4584-b36d-5501b8e06fa8", "manager", "MANAGER" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_Articles_ArticleId",
                table: "UserArticles");

            migrationBuilder.DropForeignKey(
                name: "FK_UserArticles_AspNetUsers_UserId",
                table: "UserArticles");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserArticles",
                table: "UserArticles");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "64fa95a9-a846-499b-a0e1-d302f1b03a86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a2ea3173-fb17-4fc2-8c6e-f3d5791cfdd5");

            migrationBuilder.RenameTable(
                name: "UserArticles",
                newName: "UserOwnArticlesLinking");

            migrationBuilder.RenameIndex(
                name: "IX_UserArticles_ArticleId",
                table: "UserOwnArticlesLinking",
                newName: "IX_UserOwnArticlesLinking_ArticleId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserOwnArticlesLinking",
                table: "UserOwnArticlesLinking",
                columns: new[] { "UserId", "ArticleId" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "04ca4593-a2c9-4d7c-a7b7-d42bb04a47b8", "532dbcab-581d-4929-91c9-e9742996518a", "root_admin", "ROOT_ADMIN" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fd724de2-7465-49eb-bbd4-8599d388711f", "cf80d98e-1de0-4d96-849f-e853de2527a4", "manager", "MANAGER" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserOwnArticlesLinking_Articles_ArticleId",
                table: "UserOwnArticlesLinking",
                column: "ArticleId",
                principalTable: "Articles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserOwnArticlesLinking_AspNetUsers_UserId",
                table: "UserOwnArticlesLinking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
