using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserLanguageQueries
{
    public class UserLanguageListQuery : OwnerCollectionQuery<ULLQ_Response> { }

    public class ULLQ_Response
    {
        public ULLQ_LKP_Language Language { get; set; } = null!;
        public ULLQ_LKP_LanguageProficiency? LanguageProficiency { get; set; }
    }

    public class ULLQ_LKP_Language
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class ULLQ_LKP_LanguageProficiency
    {
        public Guid ID { get; set; }
        public string Level { get; set; } = string.Empty;
    }
}
