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
    public class UserInfoQueryHandler : IRequestHandler<UserInfoQuery, SingleQueryResponse<UIQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public UserInfoQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<SingleQueryResponse<UIQ_Response>> Handle(UserInfoQuery request, CancellationToken cancellationToken)
        {
            var response = new SingleQueryResponse<UIQ_Response>();

            try
            {
                var existingEntity = await _context.User
                    .Where(u => u.ID == _currentUserService.UserID)
                    .ProjectTo<UIQ_Response>(_mapper.ConfigurationProvider)
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
