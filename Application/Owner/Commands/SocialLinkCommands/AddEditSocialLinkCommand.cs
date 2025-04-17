using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SocialLinkCommands
{
    public class AddEditSocialLinkCommand : IRequest<AbstractViewModel>
    {
        public Guid? ID { get; set; }
        public string Platform { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
    }
}
