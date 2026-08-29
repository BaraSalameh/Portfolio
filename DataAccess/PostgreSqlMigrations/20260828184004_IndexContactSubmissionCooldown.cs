using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class IndexContactSubmissionCooldown : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                SET LOCAL lock_timeout = '5s';
                SET LOCAL statement_timeout = '5min';
                CREATE INDEX "IX_ContactMessage_SubmissionCooldown"
                    ON "ContactMessage" ("UserID", "Email", "CreatedAt")
                    WHERE "IsDeleted" = false;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_ContactMessage_SubmissionCooldown",
                table: "ContactMessage");
        }
    }
}
