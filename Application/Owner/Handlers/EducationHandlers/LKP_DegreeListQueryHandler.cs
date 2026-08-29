using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Common.Text;

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
            Expression<Func<LKP_Degree, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                Filter = f =>
                    f.Name.ToLower().Contains(search) ||
                    (f.Abbreviation ?? "").ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Degree
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_DLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
