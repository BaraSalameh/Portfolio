using Application.Common.Entities;
using Application.Owner.Queries.LKP_LanguageQuieries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class RolesListQueryHandler : IRequestHandler<LKP_LanguageListQuery, ListQueryResponse<LKPLLQ_Response>>
    {

        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public RolesListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKPLLQ_Response>> Handle(LKP_LanguageListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKPLLQ_Response>();

            var existingEntity = _context.LKP_Language;

            response.Items = await _mapper.ProjectTo<LKPLLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}