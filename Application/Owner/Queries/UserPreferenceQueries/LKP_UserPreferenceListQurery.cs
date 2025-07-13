using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserPreferenceQueries
{
    public class LKP_PreferenceListQurery : IRequest<ListQueryResponse<LKP_PLQ_Response>> { }

    public class LKP_PLQ_Response
    {
        public Guid? ID { get; set; }
        public string? Name { get; set; }
    }
}
