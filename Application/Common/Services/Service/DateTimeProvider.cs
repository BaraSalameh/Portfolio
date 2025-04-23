using Application.Common.Services.Interface;

namespace Application.Common.Services.Service
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
