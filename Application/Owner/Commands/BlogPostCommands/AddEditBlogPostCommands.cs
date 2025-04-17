using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.BlogPostCommands
{
    public class AddEditBlogPostCommand : IRequest<AbstractViewModel>
    {
        public Guid? ID { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public DateOnly PublishedAt { get; set; }
    }
}
