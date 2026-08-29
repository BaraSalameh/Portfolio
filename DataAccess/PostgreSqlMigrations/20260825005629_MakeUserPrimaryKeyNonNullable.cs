using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class MakeUserPrimaryKeyNonNullable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Metadata-only correction: PostgreSQL primary keys are already NOT
            // NULL and the existing column already uses gen_random_uuid().
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Reverting CLR nullability must not weaken the database primary key
            // or remove its UUID default.
        }
    }
}
