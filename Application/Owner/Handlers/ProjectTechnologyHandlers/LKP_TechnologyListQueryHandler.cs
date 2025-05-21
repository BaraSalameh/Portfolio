using Application.Common.Entities;
using Application.Owner.Queries.ProjectTechnologyQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class LKP_TechnologyListQueryHandler : IRequestHandler<LKP_TechnologyListQuery, ListQueryResponse<LKP_TLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_TechnologyListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_TLQ_Response>> Handle(LKP_TechnologyListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_TLQ_Response>();

            var existingEntity = _context.LKP_Technology
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_TLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
