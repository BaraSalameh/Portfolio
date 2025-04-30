using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SocialLinkCommands
{
    public class DeleteSocialLinkCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
