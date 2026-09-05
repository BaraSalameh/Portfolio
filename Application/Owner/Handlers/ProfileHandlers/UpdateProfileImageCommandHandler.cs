using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.Profile;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile;

public sealed class UpdateProfileImageCommandHandler
    : IRequestHandler<UpdateProfileImageCommand, CommandResponse<UpdateProfileImageResponse>>
{
    private readonly IAppDbContext _context;
    private readonly ICurrentUserService _currentUser;
    private readonly ICloudinaryAssetService _cloudinaryAssets;

    public UpdateProfileImageCommandHandler(
        IAppDbContext context,
        ICurrentUserService currentUser,
        ICloudinaryAssetService cloudinaryAssets)
    {
        _context = context;
        _currentUser = currentUser;
        _cloudinaryAssets = cloudinaryAssets;
    }

    public async Task<CommandResponse<UpdateProfileImageResponse>> Handle(
        UpdateProfileImageCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CommandResponse<UpdateProfileImageResponse>();
        var userId = _currentUser.UserID!.Value;
        var user = await _context.User.FirstOrDefaultAsync(item => item.ID == userId, cancellationToken);
        if (user is null)
        {
            response.ResultType = ResultType.NotFound;
            response.lstError.Add("User not found.");
            return response;
        }

        var assetName = request.ImageKind == ProfileImageKind.ProfilePicture ? "profile" : "cover";
        var category = request.ImageKind == ProfileImageKind.ProfilePicture ? "profile-images" : "cover-photos";
        var assetFolder = $"folio/{category}/{userId:N}";
        var publicId = $"{assetFolder}/{assetName}";
        var previousUrl = request.ImageKind == ProfileImageKind.ProfilePicture
            ? user.ProfilePicture
            : user.CoverPhoto;
        var upload = await _cloudinaryAssets.UploadAsync(
            request.Content,
            publicId,
            assetFolder,
            cancellationToken);

        if (request.ImageKind == ProfileImageKind.ProfilePicture)
        {
            user.ProfilePicture = upload.Url;
        }
        else
        {
            user.CoverPhoto = upload.Url;
        }

        await _context.SaveChangesAsync(cancellationToken);
        await _cloudinaryAssets.DeleteByUrlAsync(
            previousUrl,
            cancellationToken: CancellationToken.None,
            preservePublicId: upload.PublicId);
        response.Data = new UpdateProfileImageResponse(upload.Url);
        return response;
    }
}
