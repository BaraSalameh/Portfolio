using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class ChartTypeSeedConfiguration : IEntityTypeConfiguration<LKP_ChartType>
    {
        public void Configure(EntityTypeBuilder<LKP_ChartType> builder)
        {
            builder.HasIndex(r => r.Name).IsUnique();
            builder.HasData(
                new LKP_ChartType { ID = Guid.Parse("b14a8d12-1e01-4d91-b7ae-85f2219f03aa"), Name = "Bar" },
                new LKP_ChartType { ID = Guid.Parse("c92a1e67-f510-49bb-910d-b331d4f04d47"), Name = "Pie" },
                new LKP_ChartType { ID = Guid.Parse("10b6e1ab-9d90-45ce-81fd-7258db4fae2c"), Name = "Radar" },
                new LKP_ChartType { ID = Guid.Parse("a5f1f2c3-67bd-41b2-bc0b-f1c7aa4fdab0"), Name = "Line" },
                new LKP_ChartType { ID = Guid.Parse("de5d14cf-9731-4ea1-8cf3-5b6bc7167b41"), Name = "Donut" }
            );
        }
    }
}
