using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;
using Application.Common.Validation;

namespace Application.Owner.Commands.SocialLinkCommands
{
    public class AddEditSocialLinkCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        [Required, StringLength(100)]
        public string Platform { get; set; } = string.Empty;
        [Required, StringLength(2048), Url, HttpUrl]
        public string Url { get; set; } = string.Empty;
        [StringLength(2048)]
        public string Icon { get; set; } = string.Empty;
    }
}
