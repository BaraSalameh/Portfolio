﻿using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class SkillSeedConfiguration : IEntityTypeConfiguration<LKP_Skill>
    {
        public void Configure(EntityTypeBuilder<LKP_Skill> builder)
        {
            builder.HasData(
                new LKP_Skill { ID = Guid.Parse("6bfb8a3e-1b9f-4d9d-a58d-36d967bc9c01"), Name = "C#", IconUrl = "https://cdn.example.com/icons/c-sharp.svg" },
                new LKP_Skill { ID = Guid.Parse("f02b09a0-c7a5-4f0c-9e6a-08d7c4f8ef24"), Name = "Java", IconUrl = "https://cdn.example.com/icons/java.svg" },
                new LKP_Skill { ID = Guid.Parse("69cce10e-9ecf-46e8-a831-b539a1a65149"), Name = "Python", IconUrl = "https://cdn.example.com/icons/python.svg" },
                new LKP_Skill { ID = Guid.Parse("d87a4b5c-43e6-4762-9f9b-6f7e4dc2c4e0"), Name = "JavaScript", IconUrl = "https://cdn.example.com/icons/javascript.svg" },
                new LKP_Skill { ID = Guid.Parse("5476bcee-4d61-4f0a-905f-2fa0f8a5287f"), Name = "HTML", IconUrl = "https://cdn.example.com/icons/html.svg" },
                new LKP_Skill { ID = Guid.Parse("02a3d389-06b7-4be0-a62f-7aa23e8a2de1"), Name = "CSS", IconUrl = "https://cdn.example.com/icons/css.svg" },
                new LKP_Skill { ID = Guid.Parse("cfcaa188-f289-4c33-82ab-7d2f16d4e60f"), Name = "SQL", IconUrl = "https://cdn.example.com/icons/sql.svg" },
                new LKP_Skill { ID = Guid.Parse("c9e6e1fc-5f70-453d-8a23-5fa9b69331e0"), Name = "MongoDB", IconUrl = "https://cdn.example.com/icons/mongodb.svg" },
                new LKP_Skill { ID = Guid.Parse("76a5c3f9-5b4e-4d3c-b2b2-481c44500cd4"), Name = "React", IconUrl = "https://cdn.example.com/icons/react.svg" },
                new LKP_Skill { ID = Guid.Parse("34db2c3b-59be-4b0f-a988-f816b4e2a82e"), Name = "Node.js", IconUrl = "https://cdn.example.com/icons/node-js.svg" },
                new LKP_Skill { ID = Guid.Parse("30e964e3-b1d1-4890-a632-857c33b22803"), Name = "Surgery", IconUrl = "https://cdn.example.com/icons/surgery.svg" },
                new LKP_Skill { ID = Guid.Parse("d8cf53c1-0fa2-4f10-9584-6c879e1420bc"), Name = "Radiology", IconUrl = "https://cdn.example.com/icons/radiology.svg" },
                new LKP_Skill { ID = Guid.Parse("908e1c7e-2de7-44f9-b189-146e4c6784e9"), Name = "Teaching", IconUrl = "https://cdn.example.com/icons/teaching.svg" },
                new LKP_Skill { ID = Guid.Parse("51d71c55-f93a-4b6d-94b5-5425e9f7c026"), Name = "Translation", IconUrl = "https://cdn.example.com/icons/translation.svg" },
                new LKP_Skill { ID = Guid.Parse("c1b76b91-55ae-47b3-9241-5e6f54b54f4f"), Name = "Finance", IconUrl = "https://cdn.example.com/icons/finance.svg" },
                new LKP_Skill { ID = Guid.Parse("47b844d2-3ee5-4907-92c3-f09f5a92b3f0"), Name = "Sales", IconUrl = "https://cdn.example.com/icons/sales.svg" },
                new LKP_Skill { ID = Guid.Parse("9d53f924-48c3-4c86-8ac3-1f8d0d013e50"), Name = "Graphic Design", IconUrl = "https://cdn.example.com/icons/graphic-design.svg" },
                new LKP_Skill { ID = Guid.Parse("73f9372e-37bb-4703-9936-8f74109aa3f0"), Name = "Content Creation", IconUrl = "https://cdn.example.com/icons/content-creation.svg" },
                new LKP_Skill { ID = Guid.Parse("cb84548a-1d9d-47c6-bdb9-01e27c86720d"), Name = "Customer Service", IconUrl = "https://cdn.example.com/icons/customer-service.svg" },
                new LKP_Skill { ID = Guid.Parse("7dc2f321-70c7-4a6e-8721-3ecf3ae36745"), Name = "Law Enforcement", IconUrl = "https://cdn.example.com/icons/law-enforcement.svg" }
            );
        }
    }
}
