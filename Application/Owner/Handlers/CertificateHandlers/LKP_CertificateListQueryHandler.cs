using Application.Common.Entities;
using Application.Owner.Queries.CertificateQueries;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using Application.Common.Text;

namespace Application.Owner.Handlers.CertificateHandlers
{
    public class LKP_CertificateListQueryHandler : IRequestHandler<LKP_CertificateListQuery, ListQueryResponse<LKP_CLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_CertificateListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_CLQ_Response>> Handle(LKP_CertificateListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_CLQ_Response>();
            Expression<Func<LKP_Certificate, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = SearchTerm.Normalize(request.Search);
                Filter = f =>
                    f.Name.ToLower().Contains(search);
            }

            var existingEntity = _context.LKP_Certificate
                .AsNoTracking()
                .Where(Filter);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<LKP_CLQ_Response>(
                    existingEntity
                        .OrderBy(u => u.Name)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
