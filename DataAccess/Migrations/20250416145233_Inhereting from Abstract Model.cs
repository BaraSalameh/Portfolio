using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class InheretingfromAbstractModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LKP_Technology",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LKP_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LKP_Technology",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LKP_Technology",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "LKP_Language",
                type: "datetime2",
                nullable: true,
                defaultValueSql: "GETUTCDATE()");

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedAt",
                table: "LKP_Language",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "LKP_Language",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "LKP_Language",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LKP_Technology");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LKP_Technology");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LKP_Technology");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LKP_Technology");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "LKP_Language");

            migrationBuilder.DropColumn(
                name: "DeletedAt",
                table: "LKP_Language");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "LKP_Language");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "LKP_Language");
        }
    }
}
