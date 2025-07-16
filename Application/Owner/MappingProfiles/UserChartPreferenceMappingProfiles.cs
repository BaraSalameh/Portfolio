using Application.Owner.Commands.UserChartPreferenceCommands;
using Application.Owner.Queries.UserChartPreferenceQueries;
using AutoMapper;
using Domain.Entities;

namespace Application.Owner.MappingProfiles
{
    public class UserChartPreferenceMappingProfiles : Profile
    {
        public UserChartPreferenceMappingProfiles()
        {
            CreateMap<EditUserChartPreferenceCommand, UserChartPreference>();

            CreateMap<UserChartPreference, UCPLQ_Response>()
                .ForMember(dest => dest.Widget, opt => opt.MapFrom(src => src.LKP_Widget))
                .ForMember(dest => dest.ChartType, opt => opt.MapFrom(src => src.LKP_ChartType));
            CreateMap<LKP_Widget, UCPLQ_LKP_Widget>();
            CreateMap<LKP_ChartType, UCPLQ_LKP_ChartType>();

            CreateMap<LKP_Widget, LKP_WLQ_Response>();
            CreateMap<LKP_ChartType, LKP_CTLQ_Response>();
        }
    }
}
