using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ContactMessageQueries
{
    public class ContactMessageListQuery : IRequest<CMLQ_Response>
    {
        public int PageNumber { get; set; } = 0;
        public int PageSize { get; set; } = 10;
    }
    
    public class CMLQ_Response : ListQueryResponse<CMLQ_ContactMessage>
    {
        public int UnreadContactMessageCount { get; set; }
    }

    public class CMLQ_ContactMessage
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
