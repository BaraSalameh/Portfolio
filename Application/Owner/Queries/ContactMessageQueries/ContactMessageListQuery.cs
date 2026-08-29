using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ContactMessageQueries
{
    public class ContactMessageListQuery : PaginationQuery, IRequest<CMLQ_Response> { }

    public class CMLQ_Response : ListQueryResponse<CMLQ_ContactMessage>
    {
        public int UnreadContactMessageCount { get; set; }
    }

    public class CMLQ_ContactMessage
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
