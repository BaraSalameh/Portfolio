using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class RoleSeedConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasData(
                new Role { ID = RoleIdentifiers.Admin, Name = nameof(RoleIdentifiers.Admin) },
                new Role { ID = RoleIdentifiers.Owner, Name = nameof(RoleIdentifiers.Owner) }
            );
        }
    }
}
