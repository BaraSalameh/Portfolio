using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserPreferenceQueries;
using AutoMapper;
using Application.Common.Persistence;
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

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            response.Items = await _mapper.ProjectTo<UPLQ_Response>(
                existingEntity
                    .OrderBy(entity => entity.LKP_PreferenceID)
                    .Skip(request.Offset)
                    .Take(request.PageSize))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
