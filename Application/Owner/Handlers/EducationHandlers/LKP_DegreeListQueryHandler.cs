using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

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
                var search = request.Search.ToLower();
                Filter = f =>
                    f.Name.ToLower().Contains(search) ||
                    (f.Abbreviation ?? "").ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Degree
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_DLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
