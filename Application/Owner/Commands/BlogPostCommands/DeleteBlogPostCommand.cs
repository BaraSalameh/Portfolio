using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.BlogPostCommands
{
    public class DeleteBlogPostCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
