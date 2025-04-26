using Application.Common.Services.Interface;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Application.Common.Services.Service
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Guid? UserID =>
            Guid.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : null;

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

        public string? Role => 
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Role)?.Value;

        public bool IsConfirmed =>
            bool.TryParse(_httpContextAccessor.HttpContext?.User?.FindFirst("IsConfirmed")?.Value, out var result) && result ;

        public string? Username =>
            _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value;

        public string? IpAddress
        {
            get
            {
                var context = _httpContextAccessor.HttpContext;
                var ip = context?.Request?.Headers["X-Forwarded-For"].FirstOrDefault();

                if (string.IsNullOrWhiteSpace(ip))
                    ip = context?.Connection?.RemoteIpAddress?.ToString();

                return ip ?? "unknown";
            }
        }
    }
}
