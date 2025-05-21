using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.ProjectTechnologyQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class ProjectTechnologyListQueryHandler : IRequestHandler<ProjectTechnologyListQuery, ListQueryResponse<PTLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ProjectTechnologyListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<PTLQ_Response>> Handle(ProjectTechnologyListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<PTLQ_Response>();

            var existingEntity = _context.Project
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false)
                .OrderBy(e => e.Order);

            response.Items = await _mapper.ProjectTo<PTLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
