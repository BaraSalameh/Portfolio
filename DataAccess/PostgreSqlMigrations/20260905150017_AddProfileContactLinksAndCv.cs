using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddProfileContactLinksAndCv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Address",
                table: "User",
                type: "character varying(120)",
                maxLength: 120,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CvUrl",
                table: "User",
                type: "character varying(2048)",
                maxLength: 2048,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WhatsAppNumber",
                table: "User",
                type: "character varying(16)",
                maxLength: 16,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                table: "SocialLink",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("""
                WITH ranked AS (
                    SELECT "ID", ROW_NUMBER() OVER (
                        PARTITION BY "UserID"
                        ORDER BY "CreatedAt" NULLS LAST, "ID") AS position
                    FROM "SocialLink"
                    WHERE "IsDeleted" = false
                )
                UPDATE "SocialLink" AS link
                SET "Order" = ranked.position
                FROM ranked
                WHERE link."ID" = ranked."ID";
                """);

            migrationBuilder.InsertData(
                table: "LKP_Preference",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("a9d7bd96-a809-4be8-820b-099a53e21901"), null, "show-whatsapp", null },
                    { new Guid("a9d7bd96-a809-4be8-820b-099a53e21902"), null, "show-site-links", null },
                    { new Guid("a9d7bd96-a809-4be8-820b-099a53e21903"), null, "show-cv", null }
                });

            migrationBuilder.AddCheckConstraint(
                name: "CK_SocialLink_Order",
                table: "SocialLink",
                sql: "\"Order\" >= 0");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropCheckConstraint(
                name: "CK_SocialLink_Order",
                table: "SocialLink");

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("a9d7bd96-a809-4be8-820b-099a53e21901"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("a9d7bd96-a809-4be8-820b-099a53e21902"));

            migrationBuilder.DeleteData(
                table: "LKP_Preference",
                keyColumn: "ID",
                keyValue: new Guid("a9d7bd96-a809-4be8-820b-099a53e21903"));

            migrationBuilder.DropColumn(
                name: "Address",
                table: "User");

            migrationBuilder.DropColumn(
                name: "CvUrl",
                table: "User");

            migrationBuilder.DropColumn(
                name: "WhatsAppNumber",
                table: "User");

            migrationBuilder.DropColumn(
                name: "Order",
                table: "SocialLink");
        }
    }
}
