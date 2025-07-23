using Application.Common.Entities;
using Application.Owner.Queries.UserSkillQueries;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class LKP_SkillCategoryListQueryHandler : IRequestHandler<LKP_SkillCategoryListQuery, ListQueryResponse<LKP_SCLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_SkillCategoryListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_SCLQ_Response>> Handle(LKP_SkillCategoryListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_SCLQ_Response>();
            Expression<Func<LKP_SkillCategory, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                Filter = f =>
                    f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_SkillCategory
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_SCLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
