using Application.Common.Entities;
using Application.Owner.Queries.LKP_LanguageQuieries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.RoleHandlers
{
    public class RolesListQueryHandler : IRequestHandler<ListQuery<LKP_LanguageListQuery>, ListQuery_Response<LKP_LanguageListQuery>>
    {

        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public RolesListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQuery_Response<LKP_LanguageListQuery>> Handle(ListQuery<LKP_LanguageListQuery> request, CancellationToken cancellationToken)
        {
            var Output = new ListQuery_Response<LKP_LanguageListQuery>();

            var ResultFromDB = _context.LKP_Language;

            Output.Items =  await _mapper.ProjectTo<LKP_LanguageListQuery>(ResultFromDB).ToListAsync();
            Output.RowCount = Output.Items.Count();

            return Output;
        }
    }
}