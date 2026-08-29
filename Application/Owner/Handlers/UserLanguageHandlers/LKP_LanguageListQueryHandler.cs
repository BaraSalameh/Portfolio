using Application.Common.Entities;
using Application.Owner.Queries.LKP_LanguageQuieries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Common.Text;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class LKP_LanguageListQueryHandler : IRequestHandler<LKP_LanguageListQuery, ListQueryResponse<LKP_LLQ_Response>>
    {

        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_LanguageListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_LLQ_Response>> Handle(LKP_LanguageListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_LLQ_Response>();
            Expression<Func<LKP_Language, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                Filter = f =>
                    f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Language
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_LLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
