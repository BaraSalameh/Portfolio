using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Changingskillstructureandaddingcertificatestructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LKP_Skill_LKP_SkillCategory_LKP_SkillCategoryID",
                table: "LKP_Skill");

            migrationBuilder.DropTable(
                name: "LKP_SkillCategory");

            migrationBuilder.DropIndex(
                name: "IX_LKP_Skill_LKP_SkillCategoryID",
                table: "LKP_Skill");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "Proficiency",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "LKP_SkillCategoryID",
                table: "LKP_Skill");

            migrationBuilder.AddColumn<Guid>(
                name: "CertificateID",
                table: "UserSkill",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "LKP_Certificate",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Certificate", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Certificate",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    LKP_CertificateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IssueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ExpirationDate = table.Column<DateOnly>(type: "date", nullable: true),
                    CredintialID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CredintialUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Certificate", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Certificate_LKP_Certificate_LKP_CertificateID",
                        column: x => x.LKP_CertificateID,
                        principalTable: "LKP_Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Certificate_User_UserID",
                        column: x => x.UserID,
                        principalTable: "User",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CertificateMedia",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CertificateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CertificateMedia", x => x.ID);
                    table.ForeignKey(
                        name: "FK_CertificateMedia_Certificate_CertificateID",
                        column: x => x.CertificateID,
                        principalTable: "Certificate",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LKP_Certificate",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("04274ed2-cdc7-4d3b-99ac-b18cf7f78a35"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("10284bc8-7380-4fef-8098-9e8682831787"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1059acd9-bc5b-481a-8c2a-25fbf3684636"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("111434a0-a785-476b-a402-3b0c6cbc9d0b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("11643d44-35d5-4bef-8956-78ed7d93c81a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("15acc4ef-0770-4f34-9878-7e66c321f4b6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("16760e04-59bf-4712-acc1-aee450ae9058"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1901ff1b-7724-405c-8908-48de51138df2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1b217b4c-e382-41ac-b353-6e8c25e85675"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1d0a2fb3-abce-46be-8996-40df1a2ea454"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("232b4254-96c1-4196-a21c-5a8920c11795"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("237d5521-b406-4c0f-9471-2d67804b03c3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("24052529-fced-4ad4-900e-0e5f555fe9ad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("246656fc-f27d-4f81-a508-27c2ddad3caa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("26846893-3967-43ac-b2dd-024d96a333f6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("292ef240-d26c-4d5d-881d-a9d2f3af68fe"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2ae2dced-eb42-4c80-911b-2c4685f56c84"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("30f03a14-000b-4b1d-9a51-73d24b35060f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33c51761-e3b0-483d-9be9-68ca0676c602"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("341cace1-59e2-439a-bab3-bbe4ebd9034c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("36bf89ff-0b90-43d3-91b8-1e741850545d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3c37a2aa-72a2-4bf2-901d-a77351964c57"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3cba1b91-894f-4462-bd7d-e1136fe99111"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3eda576a-4dd8-4f50-8467-e395235915e5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("415af969-6968-4d94-b7a3-03decf369eaf"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("440bcc23-5883-440b-b891-1e8671e9c64e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("44f1378e-336c-40c8-97a0-819249dc372b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("498a001c-11c7-4fcb-953d-7d7dd914d1ab"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("49a55547-a821-4f3c-b7af-7e9d76aba00f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4a67dae5-502d-450c-8020-5779577f18b2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4bf8184b-3ccc-4ab9-9da5-c58c35ccbe8e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4cfd44eb-6392-4e21-87b3-c197409d8bcb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("55a5a15c-6837-4683-9d02-f1462f34a3a5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("58851cf8-4b6c-42d7-8a15-f1a1e94cce73"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5c18acc3-b668-4711-a8b0-a4f0d78dd6a5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5e68d80a-8e65-4f66-9d8a-ea8fbb5b9760"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("60141ea8-3e51-4d67-a5a1-c17d46489d0f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6295282b-468d-471c-badc-8f04031f3c5b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("69cd4e57-b114-4185-9d5a-8e0c964f83cb"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6be5fc5d-2cd4-4c5e-863c-df56fddc55c4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6c755da9-3e17-4382-95b9-3b16156f203a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6dfbf4e0-f70c-4837-b715-ab94b56e6e9c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6e9b2a22-2244-4756-a2aa-b8fb02543822"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7091bdca-a368-4534-8e67-b6c1c26304c5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("73b04b6e-8d49-4e37-80a4-92f117c5f56f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("740ee348-3430-4fdd-a30c-0362a13c2c6d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7a70019c-c537-4980-b96f-780ca9df3701"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7a817f5f-9ad0-4364-b322-95668711e76f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7b2bcbf8-e5d9-4d9c-83d4-a4e24cbd36c7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7d488bd7-6e81-4db8-be6a-e49f9871f14d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7e9f63e8-9950-4311-af67-9918f30bb52a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("81f63198-c51d-4a47-9d1b-0793f6fb9c57"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8e48d852-1a3f-496a-b88f-a58512d6207d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9131cb00-0f96-4532-8981-3c628c5d7ce2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("91c4d43b-012d-44fa-a0eb-aa7d6c2e1222"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("945249b9-3551-43a9-a321-351f241fc920"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9617c867-4031-4883-8889-2c2ef747edf1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("96e0af6a-bebc-44c4-9f3c-47782d56e041"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9b3516b0-76db-42c0-9575-0f81b47a0ecc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9cca7b2a-4260-4363-a547-e14589113b7e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9e6f23c7-141c-4433-8b99-140b6fe9afad"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a401cc5b-3895-47cd-ac74-2a260566094c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a452b0dd-926e-4b23-be8f-02fb48481245"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a5cb53c5-adf6-4615-b022-466a253cae65"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a5e65fb4-bf2a-4c18-95a3-9f77e797b7ac"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a89c258f-2030-42bf-bb22-8ee64fc8f1ee"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ac3bec3d-845b-49ad-ab84-6f19c9e19b13"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b087e907-50a5-4cd5-af6d-4eefa5b8c486"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b1643bea-2313-4f9a-84e9-ab9602c3bb93"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b413d0a0-9fa6-4a2f-a809-24d2fe0f204a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b5d340af-31c1-4e2d-b7ba-da1c0358597a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b60d84c0-597c-4b4e-be13-a943cb162e1f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Cloud Practitioner", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b74c7439-5722-4e14-8f78-3c48c784fa7d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b7ad0428-1bd4-4170-b808-70fd5f28f512"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b7df6ea0-5dd1-4d1c-9f0b-6e4cd050f591"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b9cb0d6f-b758-4870-9c7e-16365b356fdf"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Network Associate", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("baae285e-b03b-4652-b540-1b57144dec7b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat System Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("bc9b4afa-c305-45e5-b363-f4be48dd8997"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("bfcc0b63-7b19-4f1e-878e-d8598d632809"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c33c294a-0e97-4607-950d-a40a2a51b825"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c6b3ef23-1969-409a-8e04-d911b58df8f5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c7c62473-59b7-4b4e-b659-aac885b0b6e6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d0f35ee4-9794-43e1-86bb-a37a4dfb4877"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Oracle Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d217e4d0-665e-42d8-880f-8990f737a8a5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "PMI IT Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d38e9b13-7c44-431c-bb35-5134b1ad9e73"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d7bbe8ae-3790-4d3c-aef2-cfb9a3017b43"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("dd539ebd-92e0-42b5-beae-e5c8c0b08460"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e08c0011-87d3-4849-8379-dd9558a13f85"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Certified Solutions Architect", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e542fbc3-51d9-467d-a707-82f24e4ef3c8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IBM Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f0371c53-3777-4434-9f83-8f06cd68fa3d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Microsoft Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f17f5a7d-7e8d-4fb1-a7bf-951d7b62306c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cisco Penetration Tester", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f224afc1-ce50-4704-8834-844fa0c9bc9d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f3d21817-8f72-426c-9247-c610839ec17d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "ISACA Data Engineer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f6705fec-5fa8-4bbe-ad0c-f4bb61810e0c"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "EC-Council Security Specialist", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f6aa0074-c9f4-4a65-a014-ae9465511e3a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Google Project Manager", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f6c1de1c-4e34-4f36-857c-4becf536b4e1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CompTIA Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f8a12b8b-b90b-4bda-864b-a122c3bde9f9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Cybersecurity Analyst", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fbed12c8-0c1f-462a-abc8-a51277703789"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Red Hat Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fd8d97bd-d305-4167-a734-7a791754dba1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Adobe Certified Developer", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fff5827a-221c-48d5-b7da-b338a5c1cadc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AWS Database Administrator", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserSkill_CertificateID",
                table: "UserSkill",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_LKP_CertificateID",
                table: "Certificate",
                column: "LKP_CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_Certificate_UserID",
                table: "Certificate",
                column: "UserID");

            migrationBuilder.CreateIndex(
                name: "IX_CertificateMedia_CertificateID",
                table: "CertificateMedia",
                column: "CertificateID");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Certificate_Name",
                table: "LKP_Certificate",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserSkill_Certificate_CertificateID",
                table: "UserSkill",
                column: "CertificateID",
                principalTable: "Certificate",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserSkill_Certificate_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropTable(
                name: "CertificateMedia");

            migrationBuilder.DropTable(
                name: "Certificate");

            migrationBuilder.DropTable(
                name: "LKP_Certificate");

            migrationBuilder.DropIndex(
                name: "IX_UserSkill_CertificateID",
                table: "UserSkill");

            migrationBuilder.DropColumn(
                name: "CertificateID",
                table: "UserSkill");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "UserSkill",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Proficiency",
                table: "UserSkill",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_SkillCategoryID",
                table: "LKP_Skill",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LKP_SkillCategory",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_SkillCategory", x => x.ID);
                });

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("30e964e3-b1d1-4890-a632-857c33b22803"),
                column: "LKP_SkillCategoryID",
                value: new Guid("69a0f114-dc20-48c3-86a9-00bc566dbe9e"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("34db2c3b-59be-4b0f-a988-f816b4e2a82e"),
                column: "LKP_SkillCategoryID",
                value: new Guid("411aaf81-bab6-4350-92b3-aecc4848a048"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"),
                column: "LKP_SkillCategoryID",
                value: new Guid("18baf95f-3d95-4d57-90c2-1a4e09f79a17"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("51d71c55-f93a-4b6d-94b5-5425e9f7c026"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e3f674d6-e889-4f00-abc7-c2aa1f5cfe63"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("69cce10e-9ecf-46e8-a831-b539a1a65149"),
                column: "LKP_SkillCategoryID",
                value: new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"),
                column: "LKP_SkillCategoryID",
                value: new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("73f9372e-37bb-4703-9936-8f74109aa3f0"),
                column: "LKP_SkillCategoryID",
                value: new Guid("94ec7d94-8659-4398-b502-d8c8b6eb0df5"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"),
                column: "LKP_SkillCategoryID",
                value: new Guid("411aaf81-bab6-4350-92b3-aecc4848a048"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"),
                column: "LKP_SkillCategoryID",
                value: new Guid("fc3613c3-45f0-4f62-9818-300b6253e84a"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("908e1c7e-2de7-44f9-b189-146e4c6784e9"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e291b5bb-8f29-414c-9274-4cebb29e5030"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"),
                column: "LKP_SkillCategoryID",
                value: new Guid("a4a28f15-046d-417d-9a84-0c3bff0457aa"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"),
                column: "LKP_SkillCategoryID",
                value: new Guid("18baf95f-3d95-4d57-90c2-1a4e09f79a17"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e6e97d25-04fb-43ca-9591-142b28ae2de7"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cb84548a-1d9d-47c6-bdb9-01e27c86720d"),
                column: "LKP_SkillCategoryID",
                value: new Guid("a24edfa2-5ca4-4c2b-8ac0-8a7ec8b504e1"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e6e97d25-04fb-43ca-9591-142b28ae2de7"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"),
                column: "LKP_SkillCategoryID",
                value: new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"),
                column: "LKP_SkillCategoryID",
                value: new Guid("b1ef00ae-d1a5-43a1-92e9-f0e11c958e1f"));

            migrationBuilder.UpdateData(
                table: "LKP_Skill",
                keyColumn: "ID",
                keyValue: new Guid("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"),
                column: "LKP_SkillCategoryID",
                value: new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"));

            migrationBuilder.InsertData(
                table: "LKP_SkillCategory",
                columns: new[] { "ID", "DeletedAt", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("0318f3fa-5e3d-45ae-8592-e682c5159a08"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Web Design", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("18baf95f-3d95-4d57-90c2-1a4e09f79a17"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1adad8a4-23e3-44d8-a743-f0e20973908e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Civil Engineering", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("1d3c0ea7-92d3-42f2-9b44-0644491f65e5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Photography", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("2410df1d-84a9-42b2-aeb5-76e57cfb0f83"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Healthcare IT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("35b66308-7673-47f7-8ed3-2573e93373d9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Psychology", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("396b62aa-5d08-4ff4-bde7-26165f3f8b7b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Architecture", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("3c05e8ec-75e4-4c2c-a512-011d8a9fdc94"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Agile Methodologies", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("40dcad4e-f5b6-4c21-a8c0-f8db27c5f8a7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Scripting", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Backend", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("411aaf81-bab6-4350-92b3-aecc4848a048"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Fullstack", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4216d2e3-6d69-48dc-97a2-b11e36cd4c8f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mechanical", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4462bb14-cf77-4cb5-a296-51d2a84b3900"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Lab Technician", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("459c810f-7504-4876-bc19-3ff6042a09fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Game Development", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("45c30ef6-5cd5-4e4f-b22d-23d1e3d67e3d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Writing", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4b6ab9bc-9b38-4761-a7b4-ef58f648e4d7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Version Control", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4c95f18d-9b8e-4dc5-b025-1b2cd4213b79"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Music", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4e8b5d3a-204c-4e27-8fcb-cf8051dd03a1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Soft Skills", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("4fd91c5c-b0f5-4a69-8c5d-5b22421477d2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Electrical Engineering", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5084a1f2-7745-4e2f-bf52-06566ec6df30"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Testing", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("52dcce06-6f7d-4e47-b882-748400bc0406"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accounting", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5c18b84f-6936-4c97-8b8e-18f296426e4b"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("65c5c20a-ef9d-4b31-9b72-b2c476c47180"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Containerization", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("69a0f114-dc20-48c3-86a9-00bc566dbe9e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Surgery", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6b7e2d2d-f678-4d7a-9d31-e1b3558a6d1f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AR/VR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6df067f9-3435-4965-9612-85ab9625c401"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Embedded Systems", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("74b75091-7698-447c-b987-32ab99b0a149"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Journalism", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("79fd8e8f-c90c-4b84-bb20-99a788f389fa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Big Data", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("812ab1b2-8d6f-4fd8-b6c7-7dfc16dc4eec"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Dentistry", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("857a537f-b59b-4a0b-a219-1852163a8367"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("8f582a53-fb45-4f5e-993b-2c6a8c1e2cf7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Education", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("94ec7d94-8659-4398-b502-d8c8b6eb0df5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Content Creation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("964de1e7-9a0a-429c-98e5-c4acfaef1a52"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Data Science", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("985c21d0-965e-47d7-ad35-aa23ebebf8d5"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Security", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9db45263-88d6-4a83-a06e-ff6d7d4723c6"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Networking", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a1c7cb1a-7ea4-44f6-9d77-9c6a6547e3b3"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Radiology", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a24edfa2-5ca4-4c2b-8ac0-8a7ec8b504e1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Customer Service", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("a4a28f15-046d-417d-9a84-0c3bff0457aa"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Graphic Design", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ad89cc4f-0bcd-465b-9752-ea2b9e511360"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Physiotherapy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ae365cd4-3261-4661-a392-f905844b396a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "UI/UX", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("ae76dc00-2b55-40f5-8dbe-d6ae24935257"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Performance Tuning", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("aec50318-c998-48b2-a19f-7aa88ebd1d74"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Film Production", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b1ef00ae-d1a5-43a1-92e9-f0e11c958e1f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Medical", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b402f1ac-dad5-4307-8313-21465e4bb54a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "AI/ML", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("b94d63b3-e443-4b13-9760-e00a9f78b1e2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nursing", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c26ce02f-7987-4b4f-a858-c3341fe85bc0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Technical Support", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c7e2b027-3f84-4c9b-b18c-0d13a4299e40"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Monitoring", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c8f20f1e-1469-441e-a597-70853a8ae14e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Blockchain", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cb80e416-0c6d-4c4c-823d-8a74d1b406c8"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Project Management", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cb84e441-d24e-40a1-850b-3a8cc48f2879"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sales", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cf963b4a-421b-41a5-bc4d-6d2101ec64c9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d03cd16a-8396-4cb1-b42d-964c4534a9d1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pharmacy", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d2ea5eec-72e2-439d-b756-916b3236e1e1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cloud", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d82b3345-d4dc-4df1-9f37-dc5c984f2958"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "IoT", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("db118146-c97f-4b8e-8511-f77ea0a5d5b2"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Cybersecurity", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("dc6c2fa6-9e0f-439d-b7fd-61e6ac51110f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Public Speaking", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e0a8695a-426f-4e1e-bd76-106c17472565"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Automation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e291b5bb-8f29-414c-9274-4cebb29e5030"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Teaching", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e3f674d6-e889-4f00-abc7-c2aa1f5cfe63"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Translation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Frontend", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e6e97d25-04fb-43ca-9591-142b28ae2de7"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Database", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e7306437-ff94-484f-8ec9-374f614945f1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "DevOps", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("e99fd15b-5a3c-42fd-8d5b-154b8651a64e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Legal", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f5ff7b30-9424-4a36-bc2e-6e7d3e9c9462"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Build Tools", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fc3613c3-45f0-4f62-9818-300b6253e84a"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Law Enforcement", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("fcf269b1-b03b-4782-9c95-87e3f8b3e8e9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mobile", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LKP_Skill_LKP_SkillCategoryID",
                table: "LKP_Skill",
                column: "LKP_SkillCategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_LKP_SkillCategory_Name",
                table: "LKP_SkillCategory",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LKP_Skill_LKP_SkillCategory_LKP_SkillCategoryID",
                table: "LKP_Skill",
                column: "LKP_SkillCategoryID",
                principalTable: "LKP_SkillCategory",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
