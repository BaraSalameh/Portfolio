using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class FieldOfStudySeedConfiguration : IEntityTypeConfiguration<LKP_FieldOfStudy>
    {
        public void Configure(EntityTypeBuilder<LKP_FieldOfStudy> builder)
        {
            builder.HasIndex(f => f.Name).IsUnique();

            builder.HasData(
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10001"), Name = "Computer Science" },
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10002"), Name = "Business Administration" },
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10003"), Name = "Electrical Engineering" },
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10004"), Name = "Mechanical Engineering" },
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10005"), Name = "Economics" },
                new LKP_FieldOfStudy { ID = Guid.Parse("9d9f3f30-1122-4b21-8a23-76a9b1b10006"), Name = "Cyber Security" }
            );
        }
    }
}
