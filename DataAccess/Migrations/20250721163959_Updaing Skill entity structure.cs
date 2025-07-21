using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UpdaingSkillentitystructure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Skill");

            migrationBuilder.RenameColumn(
                name: "IconUrl",
                table: "Skill",
                newName: "Description");

            migrationBuilder.AddColumn<Guid>(
                name: "LKP_SkillID",
                table: "Skill",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "LKP_SkillCategory",
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
                    table.PrimaryKey("PK_LKP_SkillCategory", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "LKP_Skill",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uniqueidentifier", nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconUrl = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LKP_SkillCategoryID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DeletedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LKP_Skill", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LKP_Skill_LKP_SkillCategory_LKP_SkillCategoryID",
                        column: x => x.LKP_SkillCategoryID,
                        principalTable: "LKP_SkillCategory",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.InsertData(
                table: "LKP_Skill",
                columns: new[] { "ID", "DeletedAt", "IconUrl", "LKP_SkillCategoryID", "Name", "UpdatedAt" },
                values: new object[,]
                {
                    { new Guid("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/css.svg", new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"), "CSS", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("30e964e3-b1d1-4890-a632-857c33b22803"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/surgery.svg", new Guid("69a0f114-dc20-48c3-86a9-00bc566dbe9e"), "Surgery", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("34db2c3b-59be-4b0f-a988-f816b4e2a82e"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/node-js.svg", new Guid("411aaf81-bab6-4350-92b3-aecc4848a048"), "Node.js", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/sales.svg", new Guid("18baf95f-3d95-4d57-90c2-1a4e09f79a17"), "Sales", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("51d71c55-f93a-4b6d-94b5-5425e9f7c026"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/translation.svg", new Guid("e3f674d6-e889-4f00-abc7-c2aa1f5cfe63"), "Translation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/html.svg", new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"), "HTML", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("69cce10e-9ecf-46e8-a831-b539a1a65149"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/python.svg", new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"), "Python", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/c-sharp.svg", new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"), "C#", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("73f9372e-37bb-4703-9936-8f74109aa3f0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/content-creation.svg", new Guid("94ec7d94-8659-4398-b502-d8c8b6eb0df5"), "Content Creation", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/react.svg", new Guid("411aaf81-bab6-4350-92b3-aecc4848a048"), "React", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/law-enforcement.svg", new Guid("fc3613c3-45f0-4f62-9818-300b6253e84a"), "Law Enforcement", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("908e1c7e-2de7-44f9-b189-146e4c6784e9"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/teaching.svg", new Guid("e291b5bb-8f29-414c-9274-4cebb29e5030"), "Teaching", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/graphic-design.svg", new Guid("a4a28f15-046d-417d-9a84-0c3bff0457aa"), "Graphic Design", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/finance.svg", new Guid("18baf95f-3d95-4d57-90c2-1a4e09f79a17"), "Finance", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/mongodb.svg", new Guid("e6e97d25-04fb-43ca-9591-142b28ae2de7"), "MongoDB", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cb84548a-1d9d-47c6-bdb9-01e27c86720d"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/customer-service.svg", new Guid("a24edfa2-5ca4-4c2b-8ac0-8a7ec8b504e1"), "Customer Service", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/sql.svg", new Guid("e6e97d25-04fb-43ca-9591-142b28ae2de7"), "SQL", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/javascript.svg", new Guid("e566ee68-52cb-437d-bcfa-3410f7ef0d84"), "JavaScript", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/radiology.svg", new Guid("b1ef00ae-d1a5-43a1-92e9-f0e11c958e1f"), "Radiology", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "https://cdn.example.com/icons/java.svg", new Guid("411aaf81-bab6-4350-92b3-aecc4848a047"), "Java", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Skill_LKP_SkillID",
                table: "Skill",
                column: "LKP_SkillID");

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
                name: "FK_Skill_LKP_Skill_LKP_SkillID",
                table: "Skill",
                column: "LKP_SkillID",
                principalTable: "LKP_Skill",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Skill_LKP_Skill_LKP_SkillID",
                table: "Skill");

            migrationBuilder.DropTable(
                name: "LKP_Skill");

            migrationBuilder.DropTable(
                name: "LKP_SkillCategory");

            migrationBuilder.DropIndex(
                name: "IX_Skill_LKP_SkillID",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "LKP_SkillID",
                table: "Skill");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Skill",
                newName: "IconUrl");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Skill",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Skill",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
