using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserChartPreferenceQueries
{
    public class UserChartPreferenceListQuery : IRequest<ListQueryResponse<UCPLQ_Response>> { }

    public class UCPLQ_Response
    {
        public UCPLQ_LKP_Widget Widget { get; set; }
        public UCPLQ_LKP_ChartType ChartType { get; set; }
        public string GroupBy { get; set; }
        public string? ValueSource { get; set; }
    }

    public class UCPLQ_LKP_Widget
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }

    public class UCPLQ_LKP_ChartType
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
