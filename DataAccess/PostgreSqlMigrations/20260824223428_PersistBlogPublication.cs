using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class PersistBlogPublication : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM "BlogPost"
                        WHERE "IsDeleted" = false
                        GROUP BY "UserID", "Slug" HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot enforce active blog slug uniqueness: duplicate owner slugs exist.';
                    END IF;
                END $$;
                """);

            migrationBuilder.AddColumn<DateTime>(
                name: "PublishedAt",
                table: "BlogPost",
                type: "date",
                nullable: true);

            migrationBuilder.Sql(
                "UPDATE \"BlogPost\" SET \"PublishedAt\" = COALESCE(\"CreatedAt\"::date, CURRENT_DATE);");

            migrationBuilder.AlterColumn<DateTime>(
                name: "PublishedAt",
                table: "BlogPost",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "date",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_UserID_Slug",
                table: "BlogPost",
                columns: new[] { "UserID", "Slug" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_BlogPost_UserID_Slug",
                table: "BlogPost");

            migrationBuilder.DropColumn(
                name: "PublishedAt",
                table: "BlogPost");
        }
    }
}
