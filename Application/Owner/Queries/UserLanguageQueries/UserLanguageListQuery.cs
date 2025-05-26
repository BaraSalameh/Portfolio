using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserLanguageQueries
{
    public class UserLanguageListQuery : IRequest<ListQueryResponse<ULLQ_Response>> { }

    public class ULLQ_Response
    {
        public ULLQ_LKP_Language Language { get; set; }
        public ULLQ_LKP_LanguageProficiency? LanguageProficiency { get; set; }
    }

    public class ULLQ_LKP_Language
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class ULLQ_LKP_LanguageProficiency
    {
        public Guid ID { get; set; }
        public string Level { get; set; }
    }
}
