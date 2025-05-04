using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Queries.UserQueries;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserHandlers
{
    public class UserQueryHandler : IRequestHandler<UserQuery, SingleQueryResponse<UQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<SingleQueryResponse<UQ_Response>> Handle(UserQuery request, CancellationToken cancellationToken)
        {
            var response = new SingleQueryResponse<UQ_Response>();

            try
            {
                var existingEntity = await _context.User
                    .Where(u => u.ID == _currentUserService.UserID)
                    .ProjectTo<UQ_Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("User not found.");
                    return response;
                }

                response.Data = existingEntity;
            }
            catch
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
