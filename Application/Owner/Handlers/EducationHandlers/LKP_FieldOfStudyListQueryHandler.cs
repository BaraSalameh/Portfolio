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
            Expression<Func<LKP_FieldOfStudy, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                Filter = f =>
                    f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_FieldOfStudy
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_FOSLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
