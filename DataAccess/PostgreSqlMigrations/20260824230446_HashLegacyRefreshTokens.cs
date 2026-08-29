using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class HashLegacyRefreshTokens : Migration
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
                        FROM "RefreshToken"
                        GROUP BY CASE
                            WHEN "Token" ~ '^[0-9A-F]{64}$' THEN "Token"
                            ELSE UPPER(ENCODE(SHA256(CONVERT_TO("Token", 'UTF8')), 'hex'))
                        END
                        HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot hash legacy refresh tokens: transformed duplicates exist.';
                    END IF;
                END $$;

                UPDATE "RefreshToken"
                SET "Token" = UPPER(ENCODE(SHA256(CONVERT_TO("Token", 'UTF8')), 'hex'))
                WHERE "Token" !~ '^[0-9A-F]{64}$';
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Hashing is intentionally irreversible. Roll back only to an application
            // release whose explicit migration-compatibility switch accepts hashed rows.
        }
    }
}
