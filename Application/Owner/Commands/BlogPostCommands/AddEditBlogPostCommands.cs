using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validation;

namespace Application.Owner.Commands.BlogPostCommands
{
    public class AddEditBlogPostCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;
        [Required, StringLength(200), RegularExpression("^[a-z0-9]+(?:-[a-z0-9]+)*$")]
        public string Slug { get; set; } = string.Empty;
        [Required, StringLength(100000)]
        public string Content { get; set; } = string.Empty;
        [StringLength(2048), Url, HttpUrl]
        public string Thumbnail { get; set; } = string.Empty;
        public DateOnly PublishedAt { get; set; }
        public Guid? LKP_BlogPostStatusID { get; set; }
    }
}
