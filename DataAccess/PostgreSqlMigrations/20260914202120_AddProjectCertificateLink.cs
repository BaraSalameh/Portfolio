using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.PostgreSqlMigrations
{
    /// <inheritdoc />
    public partial class AddProjectCertificateLink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "CertificateID",
                table: "Project",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Project_CertificateID",
                table: "Project",
                column: "CertificateID");

            migrationBuilder.AddForeignKey(
                name: "FK_Project_Certificate_CertificateID",
                table: "Project",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Project_Certificate_CertificateID",
                table: "Project");

            migrationBuilder.DropIndex(
                name: "IX_Project_CertificateID",
                table: "Project");

            migrationBuilder.DropColumn(
                name: "CertificateID",
                table: "Project");
        }
    }
}
