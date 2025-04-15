namespace Application.Common.Services.Interface
{
    public interface ICurrentUserService
    {
        bool IsAuthenticated { get; }
        int? UserID { get; }
        string? Role { get; }
    }
}
