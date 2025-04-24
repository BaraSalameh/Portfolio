using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class SeparatingtheEmailConfirmationlogicintoPendingEmailConfirmationentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailConfirmationToken",
                table: "User");

            migrationBuilder.DropColumn(
                name: "EmailConfirmationTokenExpiresAt",
                table: "User");

            migrationBuilder.DropColumn(
                name: "IsEmailConfirmed",
                table: "User");

            migrationBuilder.CreateTable(
                name: "PendingEmailConfirmation",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RememberMe = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingEmailConfirmation", x => x.ID);
                    table.ForeignKey(
                        name: "FK_PendingEmailConfirmation_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_Email_Token",
                table: "PendingEmailConfirmation",
                columns: new[] { "Email", "Token" });

            migrationBuilder.CreateIndex(
                name: "IX_PendingEmailConfirmation_UserID",
                table: "PendingEmailConfirmation",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PendingEmailConfirmation");

            migrationBuilder.AddColumn<string>(
                name: "EmailConfirmationToken",
                table: "User",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailConfirmationTokenExpiresAt",
                table: "User",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEmailConfirmed",
                table: "User",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
