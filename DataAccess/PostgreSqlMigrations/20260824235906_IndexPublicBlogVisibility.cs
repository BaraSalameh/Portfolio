using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class IndexPublicBlogVisibility : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_PublicVisibility",
                table: "BlogPost",
                columns: new[] { "UserID", "LKP_BlogPostStatusID", "IsDeleted", "PublishedAt", "ID" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_PublicVisibility",
                table: "BlogPost");
        }
    }
}
