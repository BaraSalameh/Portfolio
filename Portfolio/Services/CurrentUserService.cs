using Application.Common.Services.Interface;
using System.Security.Claims;

namespace Portfolio.Services;

public sealed class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
    private HttpContext? Context => httpContextAccessor.HttpContext;

    public Guid? UserID => Guid.TryParse(
        Context?.User.FindFirstValue(ClaimTypes.NameIdentifier),
        out var id) ? id : null;

    public bool IsAuthenticated => Context?.User.Identity?.IsAuthenticated ?? false;
    public string? Role => Context?.User.FindFirstValue(ClaimTypes.Role);
    public string? Username => Context?.User.FindFirstValue(ClaimTypes.Name);
    public bool IsConfirmed => bool.TryParse(Context?.User.FindFirstValue("IsConfirmed"), out var value) && value;
    public string? IpAddress => Context?.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}
