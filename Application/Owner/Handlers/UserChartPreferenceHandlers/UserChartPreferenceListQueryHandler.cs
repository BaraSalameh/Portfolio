using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserChartPreferenceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserChartPreferenceHandlers
{
    class UserChartPreferenceListQueryHandler : IRequestHandler<UserChartPreferenceListQuery, ListQueryResponse<UCPLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserChartPreferenceListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<UCPLQ_Response>> Handle(UserChartPreferenceListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<UCPLQ_Response>();

            var existingEntity = _context.UserChartPreference
                .AsNoTracking()
                .Where(ucp => ucp.UserID == _currentUserService.UserID);

            response.Items = await _mapper.ProjectTo<UCPLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
