using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class CentralizePersistenceConventions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                """
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1 FROM "User"
                        GROUP BY LOWER(BTRIM("Email")) HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot normalize email addresses: case-insensitive duplicates exist.';
                    END IF;

                    IF EXISTS (
                        SELECT 1 FROM "RefreshToken"
                        GROUP BY "Token" HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot add refresh-token uniqueness: duplicate token values exist.';
                    END IF;

                    IF EXISTS (
                        SELECT 1 FROM "PendingEmailConfirmation"
                        GROUP BY "TokenHash" HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot add confirmation-token uniqueness: duplicate token hashes exist.';
                    END IF;

                    IF EXISTS (
                        SELECT 1 FROM "UserSkill"
                        WHERE "IsDeleted" = false
                        GROUP BY "UserID", "LKP_SkillID" HAVING COUNT(*) > 1
                    ) THEN
                        RAISE EXCEPTION 'Cannot add active user-skill uniqueness: duplicate active relations exist.';
                    END IF;
                END $$;
                """);

            migrationBuilder.Sql("UPDATE \"User\" SET \"Email\" = LOWER(BTRIM(\"Email\"));");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_SocialLink_UserID",
                table: "SocialLink");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Project_UserID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropIndex(
                name: "IX_Experience_UserID",
                table: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_Education_UserID",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessage_UserID",
                table: "ContactMessage");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_UserID",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_UserID",
                table: "BlogPost");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLink_UserID_IsDeleted",
                table: "SocialLink",
                columns: new[] { "UserID", "IsDeleted" });

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken",
                column: "Token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID_IsRevoked",
                table: "RefreshToken",
                columns: new[] { "UserID", "IsRevoked" });

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserID_IsDeleted_Order_ID",
                table: "Project",
                columns: new[] { "UserID", "IsDeleted", "Order", "ID" });

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_ExpiresAt",
                table: "PendingEmailConfirmation",
                column: "ExpiresAt");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation",
                column: "TokenHash",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Experience_UserID_IsDeleted_Order_ID",
                table: "Experience",
                columns: new[] { "UserID", "IsDeleted", "Order", "ID" });

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserID_IsDeleted_Order_ID",
                table: "Education",
                columns: new[] { "UserID", "IsDeleted", "Order", "ID" });

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessage_UserID_IsDeleted_CreatedAt",
                table: "ContactMessage",
                columns: new[] { "UserID", "IsDeleted", "CreatedAt" });

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_UserID_IsDeleted_Order_ID",
                table: "Certificate",
                columns: new[] { "UserID", "IsDeleted", "Order", "ID" });

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_UserID_IsDeleted_CreatedAt",
                table: "BlogPost",
                columns: new[] { "UserID", "IsDeleted", "CreatedAt" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_SocialLink_UserID_IsDeleted",
                table: "SocialLink");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_ExpiresAt",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_Token",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_RefreshToken_UserID_IsRevoked",
                table: "RefreshToken");

            migrationBuilder.DropIndex(
                name: "IX_Project_UserID_IsDeleted_Order_ID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_ExpiresAt",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropIndex(
                name: "IX_Experience_UserID_IsDeleted_Order_ID",
                table: "Experience");

            migrationBuilder.DropIndex(
                name: "IX_Education_UserID_IsDeleted_Order_ID",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_ContactMessage_UserID_IsDeleted_CreatedAt",
                table: "ContactMessage");

            migrationBuilder.DropIndex(
                name: "IX_Certificate_UserID_IsDeleted_Order_ID",
                table: "Certificate");

            migrationBuilder.DropIndex(
                name: "IX_BlogPost_UserID_IsDeleted_CreatedAt",
                table: "BlogPost");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "RefreshToken",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID",
                table: "UserSkill",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_SocialLink_UserID",
                table: "SocialLink",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserID",
                table: "RefreshToken",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Project_UserID",
                table: "Project",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation",
                column: "TokenHash");

            migrationBuilder.CreateIndex(
                name: "IX_Experience_UserID",
                table: "Experience",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_UserID",
                table: "Education",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_ContactMessage_UserID",
                table: "ContactMessage",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_UserID",
                table: "Certificate",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPost_UserID",
                table: "BlogPost",
                column: "UserID");
        }
    }
}
