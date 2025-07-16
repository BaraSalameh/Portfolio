using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class WidgetSeedConfiguration : IEntityTypeConfiguration<LKP_Widget>
    {
        public void Configure(EntityTypeBuilder<LKP_Widget> builder)
        {
            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasData(
                new LKP_Widget { ID = Guid.Parse("a8d0f22e-d1b3-4d1f-83c7-4e67a345f311"), Name = "Education" },
                new LKP_Widget { ID = Guid.Parse("e79c20c5-92a4-47e5-b167-f028f55a364a"), Name = "Experience" },
                new LKP_Widget { ID = Guid.Parse("f3b2cf11-6ce0-4e06-b798-1826b8bc67f0"), Name = "Project" },
                new LKP_Widget { ID = Guid.Parse("55c7dd42-07ec-4c5f-aadc-2ad7f3bdfae4"), Name = "Language" },
                new LKP_Widget { ID = Guid.Parse("c6d20f43-5ae3-4df3-bf37-e657c26d63aa"), Name = "Certification" },
                new LKP_Widget { ID = Guid.Parse("b69e03a3-2fa5-4cb3-8d36-5607c49fd779"), Name = "Skill" },
                new LKP_Widget { ID = Guid.Parse("3ae5b0f3-d26c-4d98-b4ec-5c6f4b1e6f8e"), Name = "Contact" },
                new LKP_Widget { ID = Guid.Parse("194e6b38-5f1d-4b6f-bf6a-5ac4aaad5b94"), Name = "About" }
            );
        }
    }
}
