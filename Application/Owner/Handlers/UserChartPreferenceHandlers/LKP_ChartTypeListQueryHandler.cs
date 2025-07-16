using Application.Common.Entities;
using Application.Owner.Queries.UserChartPreferenceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserChartPreferenceHandlers
{
    public class LKP_ChartTypeListQueryHandler : IRequestHandler<LKP_ChartTypeListQuery, ListQueryResponse<LKP_CTLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_ChartTypeListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_CTLQ_Response>> Handle(LKP_ChartTypeListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_CTLQ_Response>();

            var existingEntity = _context.LKP_ChartType
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_CTLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
