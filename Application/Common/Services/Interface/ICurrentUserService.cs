namespace Application.Common.Services.Interface
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        Guid? UserID { get; }
        string? Role { get; }
        string? Username { get; }
        bool IsConfirmed { get; }
        string? IpAddress { get; }
    }
}
