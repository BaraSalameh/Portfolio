using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.SkillQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SkillHandlers
{
    public class SkillListQueryHandler : IRequestHandler<SkillListQuery, ListQueryResponse<SLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public SkillListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<SLQ_Response>> Handle(SkillListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<SLQ_Response>();

            var existingEntity = _context.Skill
                .AsNoTracking()
                .Where(ul => ul.UserID == _currentUserService.UserID);

            response.Items = await _mapper.ProjectTo<SLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
