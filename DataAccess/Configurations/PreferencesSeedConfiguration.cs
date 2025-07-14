using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    class PreferencesSeedConfiguration : IEntityTypeConfiguration<LKP_Preference>
    {
        public void Configure(EntityTypeBuilder<LKP_Preference> builder)
        {
            builder.HasIndex(d => d.Name).IsUnique();

            builder.HasData(
                new LKP_Preference { ID = Guid.Parse("6f1f71a6-74b1-4ed3-b2ae-4d1137dbcb8d"), Name = "show-language-bar-chart" },
                new LKP_Preference { ID = Guid.Parse("f45d65cf-2f6e-4a42-b25a-11eb326c8f38"), Name = "show-phone-number" },
                new LKP_Preference { ID = Guid.Parse("cb4c589b-cb07-4414-92f5-98d5c08867a7"), Name = "show-education-pie-chart" },
                new LKP_Preference { ID = Guid.Parse("f9ef68e1-f315-4a3c-b3d5-9a53646e75aa"), Name = "show-gender" },
                new LKP_Preference { ID = Guid.Parse("0ddf9055-fd64-4c9a-84f0-5d8db3db17a0"), Name = "show-overview-bar-chart" },
                new LKP_Preference { ID = Guid.Parse("01278046-dcf6-4c39-a256-32f52e0b6eeb"), Name = "show-experience-radar-chart" },
                new LKP_Preference { ID = Guid.Parse("ec47f4b3-2852-4067-a2e9-0e43b2e7b91b"), Name = "show-email-address" },
                new LKP_Preference { ID = Guid.Parse("95b5f7ec-e1c2-446f-8401-e0a982a6172e"), Name = "show-experience-pie-chart" },
                new LKP_Preference { ID = Guid.Parse("3004c55b-16b9-4fa2-bbe5-fbd26aa31497"), Name = "show-project-radar-chart" },
                new LKP_Preference { ID = Guid.Parse("b65b26c1-d9c7-4089-9ae2-31a2353cf434"), Name = "show-project-widget" },
                new LKP_Preference { ID = Guid.Parse("491b6c0a-7f16-4c01-b3fd-5010ff4b6072"), Name = "birthdate-format" },
                new LKP_Preference { ID = Guid.Parse("fe5d6427-2ae3-49c5-b94e-8b0e1c361471"), Name = "show-project-bar-chart" },
                new LKP_Preference { ID = Guid.Parse("acef54ff-49b5-45bb-a84f-0eafce08730c"), Name = "show-overview-radar-chart" },
                new LKP_Preference { ID = Guid.Parse("435a47b5-43c4-4c0f-91ed-7d6a32ae5398"), Name = "show-overview-pie-chart" },
                new LKP_Preference { ID = Guid.Parse("a68cd4c7-b0fd-4d25-a32f-d7772082ae9c"), Name = "show-language-pie-chart" },
                new LKP_Preference { ID = Guid.Parse("3055cce6-4022-4c2c-87cf-2ea06b9e7d2d"), Name = "show-overview-widget" },
                new LKP_Preference { ID = Guid.Parse("f1a529dc-99a1-41d1-86bb-bd9d661a9435"), Name = "show-language-radar-chart" },
                new LKP_Preference { ID = Guid.Parse("4073e1e3-3d59-4f12-ae90-31f2d20cf68b"), Name = "profile-width" },
                new LKP_Preference { ID = Guid.Parse("8f9e7e6b-6f49-420e-8fd2-3ea35aa9d5b0"), Name = "profile-picture-position" },
                new LKP_Preference { ID = Guid.Parse("ca2375d4-d3e4-4dc3-b25c-9dc6fcb03c4e"), Name = "show-education-bar-chart" },
                new LKP_Preference { ID = Guid.Parse("6d83cb36-fd8e-4fd2-87d2-4d4d9b9e4f27"), Name = "show-experience-bar-chart" },
                new LKP_Preference { ID = Guid.Parse("3f87d7a5-5ab8-4ea6-85c7-eed6bb83dcb0"), Name = "show-education-radar-chart" },
                new LKP_Preference { ID = Guid.Parse("8e4d5b5f-3f44-49a8-83c2-d4c3c5155e63"), Name = "show-birthdate" },
                new LKP_Preference { ID = Guid.Parse("d05c7c4e-c3bb-4422-8ad2-3d10ec961a49"), Name = "show-project-pie-chart" }
            );
        }
    }
}
