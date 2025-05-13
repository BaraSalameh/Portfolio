using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class LKP_DegreeListQueryHandler : IRequestHandler<LKP_DegreeListQuery, ListQueryResponse<LKP_DLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_DegreeListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_DLQ_Response>> Handle(LKP_DegreeListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_DLQ_Response>();

            var existingEntity = _context.LKP_Degree
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_DLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
