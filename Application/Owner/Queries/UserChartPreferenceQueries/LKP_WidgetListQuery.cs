using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserChartPreferenceQueries
{
    public class LKP_WidgetListQuery : IRequest<ListQueryResponse<LKP_WLQ_Response>> { }

    public class LKP_WLQ_Response
    {
        public Guid? ID { get; set; }
        public string? Name { get; set; }
    }
}
