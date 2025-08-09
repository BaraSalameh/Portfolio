using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdatingUserSkilltoacceptmanyeduexpprojandcert : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Certificate_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Education_EducationID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Experience_ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Project_ProjectID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_EducationID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_ProjectID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_EducationID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ProjectID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "CertificateID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "EducationID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "ExperienceID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "ProjectID",
                table: "UserSkill");

            migrationBuilder.CreateTable(
                name: "UserSkillCertificate",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillCertificate", x => new { x.UserSkillID, x.CertificateID });
                    table.ForeignKey(
                        name: "FK_UserSkillCertificate_Certificate_CertificateID",
                        column: x => x.CertificateID,
                        principalTable: "Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillCertificate_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillEducation",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EducationID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillEducation", x => new { x.UserSkillID, x.EducationID });
                    table.ForeignKey(
                        name: "FK_UserSkillEducation_Education_EducationID",
                        column: x => x.EducationID,
                        principalTable: "Education",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillEducation_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillExperience",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExperienceID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillExperience", x => new { x.UserSkillID, x.ExperienceID });
                    table.ForeignKey(
                        name: "FK_UserSkillExperience_Experience_ExperienceID",
                        column: x => x.ExperienceID,
                        principalTable: "Experience",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillExperience_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserSkillProject",
                columns: table => new
                {
                    UserSkillID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkillProject", x => new { x.UserSkillID, x.ProjectID });
                    table.ForeignKey(
                        name: "FK_UserSkillProject_Project_ProjectID",
                        column: x => x.ProjectID,
                        principalTable: "Project",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkillProject_UserSkill_UserSkillID",
                        column: x => x.UserSkillID,
                        principalTable: "UserSkill",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID",
                table: "UserSkill",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillCertificate_CertificateID",
                table: "UserSkillCertificate",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillEducation_EducationID",
                table: "UserSkillEducation",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillExperience_ExperienceID",
                table: "UserSkillExperience",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkillProject_ProjectID",
                table: "UserSkillProject",
                column: "ProjectID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserSkillCertificate");

            migrationBuilder.DropTable(
                name: "UserSkillEducation");

            migrationBuilder.DropTable(
                name: "UserSkillExperience");

            migrationBuilder.DropTable(
                name: "UserSkillProject");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_UserID",
                table: "UserSkill");

            migrationBuilder.AddColumn<Guid>(
                name: "CertificateID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "EducationID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExperienceID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_CertificateID",
                table: "UserSkill",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_EducationID",
                table: "UserSkill",
                column: "EducationID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_ExperienceID",
                table: "UserSkill",
                column: "ExperienceID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_ProjectID",
                table: "UserSkill",
                column: "ProjectID");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_CertificateID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "CertificateID" },
                unique: true,
                filter: "[CertificateID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_EducationID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "EducationID" },
                unique: true,
                filter: "[EducationID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ExperienceID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "ExperienceID" },
                unique: true,
                filter: "[ExperienceID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_UserID_LKP_SkillID_ProjectID",
                table: "UserSkill",
                columns: new[] { "UserID", "LKP_SkillID", "ProjectID" },
                unique: true,
                filter: "[ProjectID] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Certificate_CertificateID",
                table: "UserSkill",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Education_EducationID",
                table: "UserSkill",
                column: "EducationID",
                principalTable: "Education",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Experience_ExperienceID",
                table: "UserSkill",
                column: "ExperienceID",
                principalTable: "Experience",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Project_ProjectID",
                table: "UserSkill",
                column: "ProjectID",
                principalTable: "Project",
                principalColumn: "ID");
        }
    }
}
