using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingDbContext : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Institution",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "LKP_Institution",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_FieldOfStudy",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Degree",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "LKP_Degree",
                columns: new[] { "ID", "Abbreviation", "Name" },
                values: new object[,]
                {
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d001"), "BSc", "Bachelor of Science" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d002"), "BA", "Bachelor of Arts" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d003"), "MSc", "Master of Science" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d004"), "MBA", "Master of Business Administration" },
                    { new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d005"), "PhD", "Doctor of Philosophy" }
                });

            migrationBuilder.InsertData(
                table: "LKP_FieldOfStudy",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10001"), "Computer Science" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10002"), "Business Administration" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10003"), "Electrical Engineering" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10004"), "Mechanical Engineering" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10005"), "Economics" },
                    { new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10006"), "Cyber Security" }
                });

            migrationBuilder.InsertData(
                table: "LKP_Institution",
                columns: new[] { "ID", "Logo", "Name" },
                values: new object[,]
                {
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"), null, "Arab American University" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"), null, "Bir Zeit University" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"), null, "University of Oxford" },
                    { new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"), null, "Üsküdar Üniversitesi" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Institution_Name",
                table: "LKP_Institution",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_FieldOfStudy_Name",
                table: "LKP_FieldOfStudy",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Degree_Name",
                table: "LKP_Degree",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_LKP_Institution_Name",
                table: "LKP_Institution");

            migrationBuilder.DropIndex(
                name: "IX_LKP_FieldOfStudy_Name",
                table: "LKP_FieldOfStudy");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Degree_Name",
                table: "LKP_Degree");

            migrationBuilder.DeleteData(
                table: "LKP_Degree",
                keyColumn: "ID",
                keyValue: new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d001"));

            migrationBuilder.DeleteData(
                table: "LKP_Degree",
                keyColumn: "ID",
                keyValue: new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d002"));

            migrationBuilder.DeleteData(
                table: "LKP_Degree",
                keyColumn: "ID",
                keyValue: new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d003"));

            migrationBuilder.DeleteData(
                table: "LKP_Degree",
                keyColumn: "ID",
                keyValue: new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d004"));

            migrationBuilder.DeleteData(
                table: "LKP_Degree",
                keyColumn: "ID",
                keyValue: new Guid("73ff5e40-1e2c-4eec-a15e-0ed2f509d005"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10001"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10002"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10003"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10004"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10005"));

            migrationBuilder.DeleteData(
                table: "LKP_FieldOfStudy",
                keyColumn: "ID",
                keyValue: new Guid("9d9f3f30-1122-4b21-8a23-76a9b1b10006"));

            migrationBuilder.DeleteData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"));

            migrationBuilder.DeleteData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"));

            migrationBuilder.DeleteData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"));

            migrationBuilder.DeleteData(
                table: "LKP_Institution",
                keyColumn: "ID",
                keyValue: new Guid("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Institution",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Logo",
                table: "LKP_Institution",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_FieldOfStudy",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "LKP_Degree",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
