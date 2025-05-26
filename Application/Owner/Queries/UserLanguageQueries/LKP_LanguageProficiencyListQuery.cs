using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserLanguageQueries
{
    public class LKP_LanguageProficiencyListQuery : IRequest<ListQueryResponse<LKP_LPLQ_Response>> { }

    public class LKP_LPLQ_Response
    {
        public Guid ID { get; set; }
        public string Level { get; set; }
    }
}
