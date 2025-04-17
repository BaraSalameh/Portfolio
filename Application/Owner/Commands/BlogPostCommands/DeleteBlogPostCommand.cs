using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.BlogPostCommands
{
    public class DeleteBlogPostCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
