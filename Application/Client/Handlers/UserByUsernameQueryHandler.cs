using Application.Client.Queries;
using Application.Common.Entities;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class UserByUsernameQueryHandler : IRequestHandler<UserByUsernameQuery, SingleQueryResponse<UBUQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _clock;

        public UserByUsernameQueryHandler(
            IAppDbContext context,
            IMapper mapper,
            IDateTimeProvider clock)
        {
            _context = context;
            _mapper = mapper;
            _clock = clock;
        }

        public async Task<SingleQueryResponse<UBUQ_Response>> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var response = new SingleQueryResponse<UBUQ_Response>();

            var existingEntity = await _context.User
                .AsNoTracking()
                .Where(u => u.Username == request.Username && u.IsConfirmed)
                .AsSplitQuery()
                .ProjectTo<UBUQ_Response>(
                    _mapper.ConfigurationProvider,
                    new
                    {
                        currentPublicDate = DateOnly.FromDateTime(_clock.UtcNow)
                    })
                .FirstOrDefaultAsync(cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("Wrong username.");
                return response;
            }

            Application.Client.PublicProfilePrivacy.Apply(existingEntity);
            response.Data = existingEntity;
            return response;
        }
    }
}
