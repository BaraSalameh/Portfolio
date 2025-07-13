using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserPreferenceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserPreferenceHandlers
{
    class UserPreferenceListQueryHandler : IRequestHandler<UserPreferenceListQuery, ListQueryResponse<UPLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserPreferenceListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<UPLQ_Response>> Handle(UserPreferenceListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<UPLQ_Response>();

            var existingEntity = _context.UserPreference
                .AsNoTracking()
                .Where(up => up.UserID == _currentUserService.UserID);

            response.Items = await _mapper.ProjectTo<UPLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
