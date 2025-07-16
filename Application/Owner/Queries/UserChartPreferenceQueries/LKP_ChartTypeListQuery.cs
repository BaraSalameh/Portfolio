using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserChartPreferenceQueries
{
    public class LKP_ChartTypeListQuery : IRequest<ListQueryResponse<LKP_CTLQ_Response>> { }

    public class LKP_CTLQ_Response
    {
        public Guid? ID { get; set; }
        public string? Name { get; set; }
    }
}
