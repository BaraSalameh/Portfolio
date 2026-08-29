using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.CertificateQueries;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers
{
    public class CertificateListQueryHandler : IRequestHandler<CertificateListQuery, ListQueryResponse<CLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public CertificateListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<CLQ_Response>> Handle(CertificateListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<CLQ_Response>();

            var existingEntity = _context.Certificate
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false)
                .OrderBy(e => e.Order);

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            response.Items = await _mapper.ProjectTo<CLQ_Response>(
                existingEntity
                    .ThenBy(entity => entity.ID)
                    .Skip(request.Offset)
                    .Take(request.PageSize))
                .ToListAsync(cancellationToken);

            return response;
        }
    }
}
