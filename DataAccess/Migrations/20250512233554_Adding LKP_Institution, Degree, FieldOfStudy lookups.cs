using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class AddingLKP_InstitutionDegreeFieldOfStudylookups : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Degree",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "FieldOfStudy",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "Institution",
                table: "Education");

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_DegreeID",
                table: "Education",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_FieldOfStudyID",
                table: "Education",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_InstitutionID",
                table: "Education",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LKP_Degree",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Degree", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_FieldOfStudy",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_FieldOfStudy", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Institution",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Logo = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Institution", x => x.ID);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_DegreeID",
                table: "Education",
                column: "LKP_DegreeID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_FieldOfStudyID",
                table: "Education",
                column: "LKP_FieldOfStudyID");

            migrationBuilder.CreateIndex(
                name: "IX_Education_LKP_InstitutionID",
                table: "Education",
                column: "LKP_InstitutionID");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Degree_Name",
                table: "LKP_Degree",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_FieldOfStudy_Name",
                table: "LKP_FieldOfStudy",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Institution_Name",
                table: "LKP_Institution",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education",
                column: "LKP_DegreeID",
                principalTable: "LKP_Degree",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education",
                column: "LKP_FieldOfStudyID",
                principalTable: "LKP_FieldOfStudy",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education",
                column: "LKP_InstitutionID",
                principalTable: "LKP_Institution",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Degree_LKP_DegreeID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_FieldOfStudy_LKP_FieldOfStudyID",
                table: "Education");

            migrationBuilder.DropForeignKey(
                name: "FK_Education_LKP_Institution_LKP_InstitutionID",
                table: "Education");

            migrationBuilder.DropTable(
                name: "LKP_Degree");

            migrationBuilder.DropTable(
                name: "LKP_FieldOfStudy");

            migrationBuilder.DropTable(
                name: "LKP_Institution");

            migrationBuilder.DropIndex(
                name: "IX_Education_LKP_DegreeID",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_Education_LKP_FieldOfStudyID",
                table: "Education");

            migrationBuilder.DropIndex(
                name: "IX_Education_LKP_InstitutionID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "LKP_DegreeID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "LKP_FieldOfStudyID",
                table: "Education");

            migrationBuilder.DropColumn(
                name: "LKP_InstitutionID",
                table: "Education");

            migrationBuilder.AddColumn<string>(
                name: "Degree",
                table: "Education",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FieldOfStudy",
                table: "Education",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Institution",
                table: "Education",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
