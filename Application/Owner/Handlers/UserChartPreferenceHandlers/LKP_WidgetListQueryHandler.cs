using Application.Common.Entities;
using Application.Owner.Queries.UserChartPreferenceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserChartPreferenceHandlers
{
    public class LKP_WidgetListQueryHandler : IRequestHandler<LKP_WidgetListQuery, ListQueryResponse<LKP_WLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_WidgetListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_WLQ_Response>> Handle(LKP_WidgetListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_WLQ_Response>();

            var existingEntity = _context.LKP_Widget
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_WLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
