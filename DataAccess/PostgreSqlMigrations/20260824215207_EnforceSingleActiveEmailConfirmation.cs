using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class EnforceSingleActiveEmailConfirmation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM "PendingEmailConfirmation"
                        WHERE "RevokedAt" IS NULL
                        GROUP BY "UserID" HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot enforce one active confirmation per user: duplicate active confirmations exist.';
                    END IF;
                END $$;
                """);

            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID",
                unique: true,
                filter: "\"RevokedAt\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID");
        }
    }
}
