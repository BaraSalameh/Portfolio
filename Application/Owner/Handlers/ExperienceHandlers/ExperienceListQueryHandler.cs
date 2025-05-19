using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.ExperienceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class ExperienceListQueryHandler : IRequestHandler<ExperienceListQuery, ListQueryResponse<ELQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ExperienceListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<ELQ_Response>> Handle(ExperienceListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<ELQ_Response>();

            var existingEntity = _context.Experience
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false)
                .OrderBy(e => e.Order);


            response.Items = await _mapper.ProjectTo<ELQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
