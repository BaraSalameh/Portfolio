using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.ProjectQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectHandlers
{
    public class ProjectListQueryHandler : IRequestHandler<ProjectListQuery, ListQueryResponse<PLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ProjectListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<PLQ_Response>> Handle(ProjectListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<PLQ_Response>();

            var existingEntity = _context.Project
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false)
                .OrderBy(e => e.Order);

            response.Items = await _mapper.ProjectTo<PLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
