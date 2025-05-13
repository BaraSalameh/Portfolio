using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class LKP_InstitutionListQueryHandler : IRequestHandler<LKP_InstitutionListQuery, ListQueryResponse<LKP_ILQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_InstitutionListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_ILQ_Response>> Handle(LKP_InstitutionListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_ILQ_Response>();

            var existingEntity = _context.LKP_Institution
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_ILQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
