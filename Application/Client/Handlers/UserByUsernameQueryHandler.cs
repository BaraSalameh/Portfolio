using Application.Client.Queries;
using Application.Common.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class UserByUsernameQueryHandler : IRequestHandler<UserByUsernameQuery, SingleQueryResponse<UBUQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UserByUsernameQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SingleQueryResponse<UBUQ_Response>> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var response = new SingleQueryResponse<UBUQ_Response>();

            try
            {
                var existingEntity = await _context.User
                    .Where(u => u.Username == request.Username)
                    .ProjectTo<UBUQ_Response>(_mapper.ConfigurationProvider)
                    .FirstOrDefaultAsync(cancellationToken);

                if(existingEntity == null)
                {
                    response.lstError.Add("Wrong username.");
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
