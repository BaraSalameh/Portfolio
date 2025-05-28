using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.ContactMessageQueries
{
    public class ContactMessageListQuery : IRequest<ListQueryResponse<CMLQ_Response>> { }
    
    public class CMLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }
        public bool IsRead { get; set; }
    }
}
