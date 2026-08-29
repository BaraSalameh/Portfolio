using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class EnforceActiveCertificateMediaUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM "CertificateMedia"
                        WHERE "IsDeleted" = false
                        GROUP BY "CertificateID", "Url"
                        HAVING count(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Duplicate active certificate media URLs must be reconciled before migration.';
                    END IF;
                END $$;
                """);

            migrationBuilder.CreateIndex(
                name: "IX_CertificateMedia_CertificateID_Url",
                table: "CertificateMedia",
                columns: new[] { "CertificateID", "Url" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CertificateMedia_CertificateID_Url",
                table: "CertificateMedia");
        }
    }
}
