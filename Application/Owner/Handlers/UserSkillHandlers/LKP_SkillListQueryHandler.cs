using Application.Common.Entities;
using Application.Owner.Queries.UserSkillQueries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Common.Text;
using Application.Common.Catalogs;

namespace Application.Owner.Handlers.UserSkillHandlers
{
    public class LKP_SkillListQueryHandler : IRequestHandler<LKP_SkillListQuery, ListQueryResponse<LKP_SLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IExternalCatalogImporter _catalogImporter;

        public LKP_SkillListQueryHandler(
            IAppDbContext context,
            IMapper mapper,
            IExternalCatalogImporter catalogImporter)
        {
            _context = context;
            _mapper = mapper;
            _catalogImporter = catalogImporter;
        }

        public async Task<ListQueryResponse<LKP_SLQ_Response>> Handle(LKP_SkillListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_SLQ_Response>();
            Expression<Func<LKP_Skill, bool>> Filter = f => f.IsActive;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                if (search.Length >= 3)
                {
                    var freshAfter = DateTime.UtcNow.AddHours(-24);
                    var freshMatchCount = await _context.LKP_Skill
                        .AsNoTracking()
                        .CountAsync(item => item.IsActive
                                            && item.Source == "ESCO"
                                            && item.LastSyncedAt >= freshAfter
                                            && item.Name.ToLower().Contains(search), cancellationToken);
                    if (freshMatchCount < request.PageSize)
                    {
                        await _catalogImporter.EnrichSkillsAsync(search, request.PageSize, cancellationToken);
                    }
                }
                Filter = f =>
                    f.IsActive && f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Skill
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_SLQ_Response>(
                    existingEntity
                        .OrderByDescending(u => u.Name.ToLower() == request.Search!.ToLower())
                        .ThenByDescending(u => u.Name.ToLower().StartsWith(request.Search!.ToLower()))
                        .ThenBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
