using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingPendingEmailConfirmationtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_Email_Token",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropColumn(
                name: "IsRevoked",
                table: "PendingEmailConfirmation");

            migrationBuilder.DropColumn(
                name: "Token",
                table: "PendingEmailConfirmation");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "PendingEmailConfirmation",
                newName: "TokenHash");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "PendingEmailConfirmation",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation",
                column: "TokenHash");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PendingEmailConfirmation_TokenHash",
                table: "PendingEmailConfirmation");

            migrationBuilder.RenameColumn(
                name: "TokenHash",
                table: "PendingEmailConfirmation",
                newName: "Email");

            migrationBuilder.AlterColumn<DateTime>(
                name: "RevokedAt",
                table: "PendingEmailConfirmation",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "PendingEmailConfirmation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsRevoked",
                table: "PendingEmailConfirmation",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "PendingEmailConfirmation",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_Email_Token",
                table: "PendingEmailConfirmation",
                columns: new[] { "Email", "Token" });
        }
    }
}
