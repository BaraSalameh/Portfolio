using Application.Account.Queries;
using Application.Common.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Account.Handlers
{
    public class LogoutQueryHandler : IRequestHandler<LogoutQuery, AbstractViewModel>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogoutQueryHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<AbstractViewModel> Handle(LogoutQuery request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            try
            {
                _httpContextAccessor.HttpContext!.Response.Cookies.Delete("AuthToken");
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("error while logging out!");
            }

            return Vm;
        }
    }
}
