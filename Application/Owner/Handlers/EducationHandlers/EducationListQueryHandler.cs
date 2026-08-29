using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class EducationListQueryHandler : IRequestHandler<EducationListQuery, ListQueryResponse<ELQ_Educations>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public EducationListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<ELQ_Educations>> Handle(EducationListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<ELQ_Educations>();

            var existingEntity = _context.Education
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false)
                .OrderBy(e => e.Order);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            response.Items = await _mapper.ProjectTo<ELQ_Educations>(
                existingEntity
                    .ThenBy(entity => entity.ID)
                    .Skip(request.Offset)
                    .Take(request.PageSize))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
