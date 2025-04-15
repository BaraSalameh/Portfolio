using Microsoft.AspNetCore.Http;

namespace Application.Common.Services
{
    public class CurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? UserID =>
            int.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("ID")?.Value, out var id) ? id : null;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
    }
}
