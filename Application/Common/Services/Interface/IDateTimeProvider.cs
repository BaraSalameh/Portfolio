namespace Application.Common.Services.Interface
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
