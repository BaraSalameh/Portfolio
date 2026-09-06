using Application.Common.Constants;
using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.SocialLinkCommands;

public sealed class SortSocialLinksCommand : IRequest<CommandResponse>
{
    [MaxLength(ProfileLimits.MaximumSocialLinks)]
    public List<Guid> SocialLinkIdsInOrder { get; set; } = [];
}
