using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using Domain.Enums;
using MediatR;

namespace Application.Account.Handlers
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, CommandResponse<VTC_Response>>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITokenRefreshService _tokenRefreshService;

        public ValidateTokenCommandHandler(ICurrentUserService currentUserService, ITokenRefreshService tokenRefreshService)
        {
            _currentUserService = currentUserService;
            _tokenRefreshService = tokenRefreshService;
        }

        public async Task<CommandResponse<VTC_Response>> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse<VTC_Response>();

            if (!_currentUserService.IsAuthenticated)
            {
                var refreshedUser = await _tokenRefreshService.TryRefreshTokenAsync(cancellationToken);

                if (refreshedUser == null)
                {
                    response.ResultType = ResultType.Unauthorized;
                    response.lstError.Add("Your session has expired. Please log in again.");
                    return response;
                }

                response.Data = new VTC_Response
                {
                    Username = refreshedUser.Username!,
                    Role = refreshedUser.Role.Name!
                };

                return response;
            }

            response.Data = new VTC_Response
            {
                Username = _currentUserService.Username!,
                Role = _currentUserService.Role!
            };
            
            return response;
        }
    }

}
