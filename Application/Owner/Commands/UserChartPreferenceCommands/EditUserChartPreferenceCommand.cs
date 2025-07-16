using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserChartPreferenceCommands
{
    public class EditUserChartPreferenceCommand : IRequest<CommandResponse>
    {
        public Guid LKP_WidgetID { get; set; }
        public Guid LKP_ChartTypeID { get; set; }
        public string GroupBy { get; set; }
        public string? ValueSource { get; set; }
    }
}
