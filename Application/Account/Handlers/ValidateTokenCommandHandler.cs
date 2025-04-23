using Application.Account.Commands;
using Application.Common.Services.Interface;
using MediatR;

namespace Application.Account.Handlers
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, VTC_Response>
    {
        private readonly ICurrentUserService _currentUserService;

        public ValidateTokenCommandHandler(ICurrentUserService currentUserService)
        {
            _currentUserService = currentUserService;
        }

        public async Task<VTC_Response> Handle(ValidateTokenCommand request, CancellationToken cancellationToken)
        {
            var Vm = new VTC_Response();

            // Use CurrentUserService to check if the user is authenticated
            if (!_currentUserService.IsAuthenticated)
            {
                Vm.status = false;
                Vm.lstError.Add("User is not authenticated.");
                return Vm;
            }

            // Use CurrentUserService to extract the username
            var username = _currentUserService.Username;

            if (string.IsNullOrEmpty(username))
            {
                Vm.status = false;
                Vm.lstError.Add("Username is not available.");
                return Vm;
            }

            // If username exists and is authenticated, return success response
            Vm.status = true;
            Vm.Username = username;

            return Vm;
        }
    }
}
