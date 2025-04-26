using Application.Account.Commands;
using Application.Common.Services.Interface;
using MediatR;

namespace Application.Account.Handlers
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, VTC_Response>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly ITokenRefreshService _tokenRefreshService;

        public ValidateTokenCommandHandler(ICurrentUserService currentUserService, ITokenRefreshService tokenRefreshService)
        {
            _currentUserService = currentUserService;
            _tokenRefreshService = tokenRefreshService;
        }

        public async Task<VTC_Response> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var Vm = new VTC_Response();

            if (!_currentUserService.IsAuthenticated)
            {
                var refreshedUser = await _tokenRefreshService.TryRefreshTokenAsync(cancellationToken);

                if (refreshedUser == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("User is not authenticated and token refresh failed.");
                    return Vm;
                }

                Vm.status = true;
                Vm.Username = refreshedUser.Username!;
                Vm.Role = refreshedUser.Role.Name!;
                Vm.IsConfirmed = refreshedUser.IsConfirmed!;
                return Vm;
            }

            Vm.status = true;
            Vm.Username = _currentUserService.Username!;
            Vm.Role = _currentUserService.Role!;
            Vm.IsConfirmed = _currentUserService.IsConfirmed;
            return Vm;
        }
    }

}
