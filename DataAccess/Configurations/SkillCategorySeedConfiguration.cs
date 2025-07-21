using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class SkillCategorySeedConfiguration : IEntityTypeConfiguration<LKP_SkillCategory>
    {
        public void Configure(EntityTypeBuilder<LKP_SkillCategory> builder)
        {
            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasData(
                new LKP_SkillCategory { ID = Guid.Parse("e566ee68-52cb-437d-bcfa-3410f7ef0d84"), Name = "Frontend" },
                new LKP_SkillCategory { ID = Guid.Parse("411aaf81-bab6-4350-92b3-aecc4848a047"), Name = "Backend" },
                new LKP_SkillCategory { ID = Guid.Parse("411aaf81-bab6-4350-92b3-aecc4848a048"), Name = "Fullstack" },
                new LKP_SkillCategory { ID = Guid.Parse("e7306437-ff94-484f-8ec9-374f614945f1"), Name = "DevOps" },
                new LKP_SkillCategory { ID = Guid.Parse("fcf269b1-b03b-4782-9c95-87e3f8b3e8e9"), Name = "Mobile" },
                new LKP_SkillCategory { ID = Guid.Parse("e6e97d25-04fb-43ca-9591-142b28ae2de7"), Name = "Database" },
                new LKP_SkillCategory { ID = Guid.Parse("d2ea5eec-72e2-439d-b756-916b3236e1e1"), Name = "Cloud" },
                new LKP_SkillCategory { ID = Guid.Parse("985c21d0-965e-47d7-ad35-aa23ebebf8d5"), Name = "Security" },
                new LKP_SkillCategory { ID = Guid.Parse("b402f1ac-dad5-4307-8313-21465e4bb54a"), Name = "AI/ML" },
                new LKP_SkillCategory { ID = Guid.Parse("459c810f-7504-4876-bc19-3ff6042a09fa"), Name = "Game Development" },
                new LKP_SkillCategory { ID = Guid.Parse("6df067f9-3435-4965-9612-85ab9625c401"), Name = "Embedded Systems" },
                new LKP_SkillCategory { ID = Guid.Parse("5084a1f2-7745-4e2f-bf52-06566ec6df30"), Name = "Testing" },
                new LKP_SkillCategory { ID = Guid.Parse("cb80e416-0c6d-4c4c-823d-8a74d1b406c8"), Name = "Project Management" },
                new LKP_SkillCategory { ID = Guid.Parse("ae365cd4-3261-4661-a392-f905844b396a"), Name = "UI/UX" },
                new LKP_SkillCategory { ID = Guid.Parse("964de1e7-9a0a-429c-98e5-c4acfaef1a52"), Name = "Data Science" },
                new LKP_SkillCategory { ID = Guid.Parse("9db45263-88d6-4a83-a06e-ff6d7d4723c6"), Name = "Networking" },
                new LKP_SkillCategory { ID = Guid.Parse("c8f20f1e-1469-441e-a597-70853a8ae14e"), Name = "Blockchain" },
                new LKP_SkillCategory { ID = Guid.Parse("79fd8e8f-c90c-4b84-bb20-99a788f389fa"), Name = "Big Data" },
                new LKP_SkillCategory { ID = Guid.Parse("4e8b5d3a-204c-4e27-8fcb-cf8051dd03a1"), Name = "Soft Skills" },
                new LKP_SkillCategory { ID = Guid.Parse("40dcad4e-f5b6-4c21-a8c0-f8db27c5f8a7"), Name = "Scripting" },
                new LKP_SkillCategory { ID = Guid.Parse("4b6ab9bc-9b38-4761-a7b4-ef58f648e4d7"), Name = "Version Control" },
                new LKP_SkillCategory { ID = Guid.Parse("0318f3fa-5e3d-45ae-8592-e682c5159a08"), Name = "Web Design" },
                new LKP_SkillCategory { ID = Guid.Parse("d82b3345-d4dc-4df1-9f37-dc5c984f2958"), Name = "IoT" },
                new LKP_SkillCategory { ID = Guid.Parse("e0a8695a-426f-4e1e-bd76-106c17472565"), Name = "Automation" },
                new LKP_SkillCategory { ID = Guid.Parse("db118146-c97f-4b8e-8511-f77ea0a5d5b2"), Name = "Cybersecurity" },
                new LKP_SkillCategory { ID = Guid.Parse("6b7e2d2d-f678-4d7a-9d31-e1b3558a6d1f"), Name = "AR/VR" },
                new LKP_SkillCategory { ID = Guid.Parse("ae76dc00-2b55-40f5-8dbe-d6ae24935257"), Name = "Performance Tuning" },
                new LKP_SkillCategory { ID = Guid.Parse("65c5c20a-ef9d-4b31-9b72-b2c476c47180"), Name = "Containerization" },
                new LKP_SkillCategory { ID = Guid.Parse("c7e2b027-3f84-4c9b-b18c-0d13a4299e40"), Name = "Monitoring" },
                new LKP_SkillCategory { ID = Guid.Parse("f5ff7b30-9424-4a36-bc2e-6e7d3e9c9462"), Name = "Build Tools" },
                new LKP_SkillCategory { ID = Guid.Parse("3c05e8ec-75e4-4c2c-a512-011d8a9fdc94"), Name = "Agile Methodologies" },
                new LKP_SkillCategory { ID = Guid.Parse("b1ef00ae-d1a5-43a1-92e9-f0e11c958e1f"), Name = "Medical" },
                new LKP_SkillCategory { ID = Guid.Parse("b94d63b3-e443-4b13-9760-e00a9f78b1e2"), Name = "Nursing" },
                new LKP_SkillCategory { ID = Guid.Parse("d03cd16a-8396-4cb1-b42d-964c4534a9d1"), Name = "Pharmacy" },
                new LKP_SkillCategory { ID = Guid.Parse("812ab1b2-8d6f-4fd8-b6c7-7dfc16dc4eec"), Name = "Dentistry" },
                new LKP_SkillCategory { ID = Guid.Parse("69a0f114-dc20-48c3-86a9-00bc566dbe9e"), Name = "Surgery" },
                new LKP_SkillCategory { ID = Guid.Parse("a1c7cb1a-7ea4-44f6-9d77-9c6a6547e3b3"), Name = "Radiology" },
                new LKP_SkillCategory { ID = Guid.Parse("4462bb14-cf77-4cb5-a296-51d2a84b3900"), Name = "Lab Technician" },
                new LKP_SkillCategory { ID = Guid.Parse("ad89cc4f-0bcd-465b-9752-ea2b9e511360"), Name = "Physiotherapy" },
                new LKP_SkillCategory { ID = Guid.Parse("35b66308-7673-47f7-8ed3-2573e93373d9"), Name = "Psychology" },
                new LKP_SkillCategory { ID = Guid.Parse("2410df1d-84a9-42b2-aeb5-76e57cfb0f83"), Name = "Healthcare IT" },
                new LKP_SkillCategory { ID = Guid.Parse("8f582a53-fb45-4f5e-993b-2c6a8c1e2cf7"), Name = "Education" },
                new LKP_SkillCategory { ID = Guid.Parse("e291b5bb-8f29-414c-9274-4cebb29e5030"), Name = "Teaching" },
                new LKP_SkillCategory { ID = Guid.Parse("e3f674d6-e889-4f00-abc7-c2aa1f5cfe63"), Name = "Translation" },
                new LKP_SkillCategory { ID = Guid.Parse("18baf95f-3d95-4d57-90c2-1a4e09f79a17"), Name = "Finance" },
                new LKP_SkillCategory { ID = Guid.Parse("52dcce06-6f7d-4e47-b882-748400bc0406"), Name = "Accounting" },
                new LKP_SkillCategory { ID = Guid.Parse("5c18b84f-6936-4c97-8b8e-18f296426e4b"), Name = "Marketing" },
                new LKP_SkillCategory { ID = Guid.Parse("cb84e441-d24e-40a1-850b-3a8cc48f2879"), Name = "Sales" },
                new LKP_SkillCategory { ID = Guid.Parse("857a537f-b59b-4a0b-a219-1852163a8367"), Name = "HR" },
                new LKP_SkillCategory { ID = Guid.Parse("e99fd15b-5a3c-42fd-8d5b-154b8651a64e"), Name = "Legal" },
                new LKP_SkillCategory { ID = Guid.Parse("fc3613c3-45f0-4f62-9818-300b6253e84a"), Name = "Law Enforcement" },
                new LKP_SkillCategory { ID = Guid.Parse("cf963b4a-421b-41a5-bc4d-6d2101ec64c9"), Name = "Engineering" },
                new LKP_SkillCategory { ID = Guid.Parse("4216d2e3-6d69-48dc-97a2-b11e36cd4c8f"), Name = "Mechanical" },
                new LKP_SkillCategory { ID = Guid.Parse("1adad8a4-23e3-44d8-a743-f0e20973908e"), Name = "Civil Engineering" },
                new LKP_SkillCategory { ID = Guid.Parse("4fd91c5c-b0f5-4a69-8c5d-5b22421477d2"), Name = "Electrical Engineering" },
                new LKP_SkillCategory { ID = Guid.Parse("396b62aa-5d08-4ff4-bde7-26165f3f8b7b"), Name = "Architecture" },
                new LKP_SkillCategory { ID = Guid.Parse("a4a28f15-046d-417d-9a84-0c3bff0457aa"), Name = "Graphic Design" },
                new LKP_SkillCategory { ID = Guid.Parse("1d3c0ea7-92d3-42f2-9b44-0644491f65e5"), Name = "Photography" },
                new LKP_SkillCategory { ID = Guid.Parse("4c95f18d-9b8e-4dc5-b025-1b2cd4213b79"), Name = "Music" },
                new LKP_SkillCategory { ID = Guid.Parse("aec50318-c998-48b2-a19f-7aa88ebd1d74"), Name = "Film Production" },
                new LKP_SkillCategory { ID = Guid.Parse("45c30ef6-5cd5-4e4f-b22d-23d1e3d67e3d"), Name = "Writing" },
                new LKP_SkillCategory { ID = Guid.Parse("94ec7d94-8659-4398-b502-d8c8b6eb0df5"), Name = "Content Creation" },
                new LKP_SkillCategory { ID = Guid.Parse("74b75091-7698-447c-b987-32ab99b0a149"), Name = "Journalism" },
                new LKP_SkillCategory { ID = Guid.Parse("dc6c2fa6-9e0f-439d-b7fd-61e6ac51110f"), Name = "Public Speaking" },
                new LKP_SkillCategory { ID = Guid.Parse("a24edfa2-5ca4-4c2b-8ac0-8a7ec8b504e1"), Name = "Customer Service" },
                new LKP_SkillCategory { ID = Guid.Parse("c26ce02f-7987-4b4f-a858-c3341fe85bc0"), Name = "Technical Support" }
            );
        }
    }
}
