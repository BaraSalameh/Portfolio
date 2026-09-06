using Application.Common.Constants;
using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SocialLinkCommands;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SocialLinkHandlers;

public sealed class SortSocialLinksCommandHandler(
    IAppDbContext context,
    ICurrentUserService currentUser) : IRequestHandler<SortSocialLinksCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(SortSocialLinksCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse();
        if (request.SocialLinkIdsInOrder.Count != request.SocialLinkIdsInOrder.Distinct().Count())
        {
            response.lstError.Add("Duplicate social link IDs are not allowed.");
            return response;
        }

        var links = await context.SocialLink
            .Where(link => link.UserID == currentUser.UserID && !link.IsDeleted)
            .Take(ProfileLimits.MaximumSocialLinks + 1)
            .ToDictionaryAsync(link => link.ID, cancellationToken);

        if (links.Count > ProfileLimits.MaximumSocialLinks
            || !links.Keys.ToHashSet().SetEquals(request.SocialLinkIdsInOrder))
        {
            response.lstError.Add("The request must contain every active social link exactly once.");
            return response;
        }

        for (var index = 0; index < request.SocialLinkIdsInOrder.Count; index++)
        {
            links[request.SocialLinkIdsInOrder[index]].Order = index + 1;
        }

        await context.SaveChangesAsync(cancellationToken);
        return response;
    }
}
