using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class InstitutionSeedConfiguration : IEntityTypeConfiguration<LKP_Institution>
    {
        public void Configure(EntityTypeBuilder<LKP_Institution> builder)
        {
            builder.HasIndex(i => i.Name).IsUnique();

            builder.HasData(
                new LKP_Institution { ID = Guid.Parse("8a43b350-6f9b-4e02-b1a1-3dfc99a1c001"), Name = "Arab American University" },
                new LKP_Institution { ID = Guid.Parse("8a43b350-6f9b-4e02-b1a1-3dfc99a1c002"), Name = "Bir Zeit University" },
                new LKP_Institution { ID = Guid.Parse("8a43b350-6f9b-4e02-b1a1-3dfc99a1c003"), Name = "University of Oxford" },
                new LKP_Institution { ID = Guid.Parse("8a43b350-6f9b-4e02-b1a1-3dfc99a1c004"), Name = "Üsküdar Üniversitesi" }
            );
        }
    }
}
