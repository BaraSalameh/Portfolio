using Application.Common.Entities;
using Application.Owner.Queries.UserSkillQueries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Common.Text;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class LKP_SkillListQueryHandler : IRequestHandler<LKP_SkillListQuery, ListQueryResponse<LKP_SLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_SkillListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_SLQ_Response>> Handle(LKP_SkillListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_SLQ_Response>();
            Expression<Func<LKP_Skill, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                Filter = f =>
                    f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Skill
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_SLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
