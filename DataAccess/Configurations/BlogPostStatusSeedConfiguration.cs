using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class BlogPostStatusSeedConfiguration : IEntityTypeConfiguration<LKP_BlogPostStatus>
    {
        public void Configure(EntityTypeBuilder<LKP_BlogPostStatus> builder)
        {
            builder.HasIndex(d => d.Name).IsUnique();

            builder.HasData(
                new LKP_BlogPostStatus { ID = Guid.Parse("d3a7b6f1-8f2a-4d93-9bfc-1e8a4b6f0a11"), Name = "Draft" },
                new LKP_BlogPostStatus { ID = Guid.Parse("4c9e2d6a-6d8b-4a2e-9f2d-32f4a7d290c3"), Name = "Published" },
                new LKP_BlogPostStatus { ID = Guid.Parse("8b1f2e0c-5b7e-4f6d-98e4-cfb230fe4f99"), Name = "Scheduled" },
                new LKP_BlogPostStatus { ID = Guid.Parse("ee4a3c1d-7f42-4f1a-b4d9-2d84f8a72954"), Name = "Archived" },
                new LKP_BlogPostStatus { ID = Guid.Parse("a7f5d9b3-9c7d-47a9-8c2e-13d43f26a6f2"), Name = "PendingReview" },
                new LKP_BlogPostStatus { ID = Guid.Parse("f12c7b8e-3a49-4b9f-9e13-6dbd85d24870"), Name = "Rejected" },
                new LKP_BlogPostStatus { ID = Guid.Parse("b8d6f4a0-1e97-4f39-80c9-3f1e7216b45e"), Name = "Deleted" }
            );
        }
    }
}
