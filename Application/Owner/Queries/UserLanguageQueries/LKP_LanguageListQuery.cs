using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.LKP_LanguageQuieries
{
    public class LKP_LanguageListQuery : IRequest<ListQueryResponse<LKP_LLQ_Response>> { }

    public class LKP_LLQ_Response
    {
        public Guid? ID { get; set; }
        public string? name { get; set; }
    }
}
