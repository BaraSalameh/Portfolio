using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class DegreeSeedConfiguration : IEntityTypeConfiguration<LKP_Degree>
    {
        public void Configure(EntityTypeBuilder<LKP_Degree> builder)
        {
            builder.HasIndex(d => d.Name).IsUnique();

            builder.HasData(
                new LKP_Degree { ID = Guid.Parse("73ff5e40-1e2c-4eec-a15e-0ed2f509d001"), Name = "Bachelor of Science", Abbreviation = "BSc" },
                new LKP_Degree { ID = Guid.Parse("73ff5e40-1e2c-4eec-a15e-0ed2f509d002"), Name = "Bachelor of Arts", Abbreviation = "BA" },
                new LKP_Degree { ID = Guid.Parse("73ff5e40-1e2c-4eec-a15e-0ed2f509d003"), Name = "Master of Science", Abbreviation = "MSc" },
                new LKP_Degree { ID = Guid.Parse("73ff5e40-1e2c-4eec-a15e-0ed2f509d004"), Name = "Master of Business Administration", Abbreviation = "MBA" },
                new LKP_Degree { ID = Guid.Parse("73ff5e40-1e2c-4eec-a15e-0ed2f509d005"), Name = "Doctor of Philosophy", Abbreviation = "PhD" }
            );
        }
    }
}
