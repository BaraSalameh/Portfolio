using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserLanguageQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class UserLanguageListQueryHandler : IRequestHandler<UserLanguageListQuery, ListQueryResponse<ULLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserLanguageListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<ListQueryResponse<ULLQ_Response>> Handle(UserLanguageListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<ULLQ_Response>();

            var existingEntity = _context.UserLanguage
                .AsNoTracking()
                .Where(ul => ul.UserID == _currentUserService.UserID);

            response.Items = await _mapper.ProjectTo<ULLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
