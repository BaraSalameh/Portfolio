using Application.Common.Entities;
using Application.Owner.Queries.UserLanguageQueries;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class LKP_LanguageProficiencyListQueryHandler : IRequestHandler<LKP_LanguageProficiencyListQuery, ListQueryResponse<LKP_LPLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_LanguageProficiencyListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_LPLQ_Response>> Handle(LKP_LanguageProficiencyListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_LPLQ_Response>();

            var existingEntity = _context.LKP_LanguageProficiency
                .AsNoTracking();

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            response.Items = await _mapper.ProjectTo<LKP_LPLQ_Response>(
                existingEntity.OrderBy(entity => entity.Level).Take(100))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
