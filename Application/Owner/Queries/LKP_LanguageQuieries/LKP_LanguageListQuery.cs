using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.LKP_LanguageQuieries
{
    public class LKP_LanguageListQuery : IRequest<ListQueryResponse<LKPLLQ_Response>> { }

    public class LKPLLQ_Response
    {
        public Guid? ID { get; set; }
        public string? name { get; set; }
    }
}
