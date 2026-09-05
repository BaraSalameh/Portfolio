using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.Profile;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile;

public sealed class RemoveProfileImageCommandHandler
    : IRequestHandler<RemoveProfileImageCommand, CommandResponse>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ICloudinaryAssetService _cloudinaryAssets;

    public RemoveProfileImageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUser,
        ICloudinaryAssetService cloudinaryAssets)
    {
        _context = context;
        _currentUser = currentUser;
        _cloudinaryAssets = cloudinaryAssets;
    }

    public async Task<CommandResponse> Handle(
        RemoveProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CommandResponse();
        var userId = _currentUser.UserID!.Value;
        var user = await _context.User.FirstOrDefaultAsync(item => item.ID == userId, cancellationToken);
        if (user is null)
        {
            response.lstError.Add("User not found.");
            return response;
        }

        var previousUrl = request.ImageKind == ProfileImageKind.ProfilePicture
            ? user.ProfilePicture
            : user.CoverPhoto;

        if (request.ImageKind == ProfileImageKind.ProfilePicture)
        {
            user.ProfilePicture = null;
        }
        else
        {
            user.CoverPhoto = null;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cloudinaryAssets.DeleteByUrlAsync(previousUrl, CancellationToken.None);
        return response;
    }
}
