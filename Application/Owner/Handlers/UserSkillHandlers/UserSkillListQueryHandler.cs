using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserSkillQueries;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class UserSkillListQueryHandler : IRequestHandler<UserSkillListQuery, ListQueryResponse<USLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserSkillListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<USLQ_Response>> Handle(UserSkillListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<USLQ_Response>();

            var existingEntity = _context.UserSkill
                .AsNoTracking()
                .Where(ul => ul.UserID == _currentUserService.UserID);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            response.Items = await _mapper.ProjectTo<USLQ_Response>(
                existingEntity
                    .OrderBy(entity => entity.ID)
                    .Skip(request.Offset)
                    .Take(request.PageSize))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
