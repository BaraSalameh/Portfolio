using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserChartPreferenceQueries
{
    public class UserChartPreferenceListQuery : OwnerCollectionQuery<UCPLQ_Response> { }

    public class UCPLQ_Response
    {
        public UCPLQ_LKP_Widget Widget { get; set; } = null!;
        public UCPLQ_LKP_ChartType ChartType { get; set; } = null!;
        public string GroupBy { get; set; } = string.Empty;
        public string? ValueSource { get; set; }
    }

    public class UCPLQ_LKP_Widget
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class UCPLQ_LKP_ChartType
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
