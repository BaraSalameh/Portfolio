using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddCustomFieldOfStudyCatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "LKP_FieldOfStudy",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<string>(
                name: "Source",
                table: "LKP_FieldOfStudy",
                type: "character varying(32)",
                maxLength: 32,
                nullable: false,
                defaultValue: "Internal");

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10001"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10002"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10003"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10004"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10005"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });

            migrationBuilder.UpdateData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10006"),
                columns: new[] { "IsActive", "Source" },
                values: new object[] { true, "Internal" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "LKP_FieldOfStudy");

            migrationBuilder.DropColumn(
                name: "Source",
                table: "LKP_FieldOfStudy");
        }
    }
}
