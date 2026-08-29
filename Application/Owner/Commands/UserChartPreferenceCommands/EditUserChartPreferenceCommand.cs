using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.UserChartPreferenceCommands
{
    public class EditUserChartPreferenceCommand : IRequest<CommandResponse>
    {
        public Guid LKP_WidgetID { get; set; }
        public Guid LKP_ChartTypeID { get; set; }
        [Required, StringLength(100)]
        public string GroupBy { get; set; } = string.Empty;
        [StringLength(200)]
        public string? ValueSource { get; set; }
    }
}
