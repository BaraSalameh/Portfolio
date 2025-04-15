using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SocialLinkCommands
{
    public class DeleteSocialLinkCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
