using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class LKP_FieldOfStudyListQueryHandler : IRequestHandler<LKP_FieldOfStudyListQuery, ListQueryResponse<LKP_FOSLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_FieldOfStudyListQueryHandler(IMapper mapper, IAppDbContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_FOSLQ_Response>> Handle(LKP_FieldOfStudyListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_FOSLQ_Response>();

            var existingEntity = _context.LKP_FieldOfStudy
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_FOSLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
