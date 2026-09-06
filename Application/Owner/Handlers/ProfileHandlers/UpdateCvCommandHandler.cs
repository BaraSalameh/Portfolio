using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.Profile;
using Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile;

public sealed class UpdateCvCommandHandler(
    IAppDbContext context,
    ICurrentUserService currentUser,
    ICloudinaryAssetService cloudinaryAssets)
    : IRequestHandler<UpdateCvCommand, CommandResponse<UpdateCvResponse>>
{
    public async Task<CommandResponse<UpdateCvResponse>> Handle(UpdateCvCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<UpdateCvResponse>();
        var user = await context.User.FirstOrDefaultAsync(item => item.ID == currentUser.UserID, cancellationToken);
        if (user is null)
        {
            response.ResultType = ResultType.NotFound;
            response.lstError.Add("User not found.");
            return response;
        }

        var folder = $"folio/cvs/{user.ID:N}";
        var upload = await cloudinaryAssets.UploadPdfAsync(
            request.Content,
            request.FileName,
            $"{folder}/cv",
            folder,
            cancellationToken);
        var previousUrl = user.CvUrl;
        user.CvUrl = upload.Url;
        await context.SaveChangesAsync(cancellationToken);
        await cloudinaryAssets.DeleteCvByUrlAsync(previousUrl, CancellationToken.None, upload.PublicId);
        response.Data = new UpdateCvResponse(upload.Url);
        return response;
    }
}

public sealed class RemoveCvCommandHandler(
    IAppDbContext context,
    ICurrentUserService currentUser,
    ICloudinaryAssetService cloudinaryAssets) : IRequestHandler<RemoveCvCommand, CommandResponse>
{
    public async Task<CommandResponse> Handle(RemoveCvCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse();
        var user = await context.User.FirstOrDefaultAsync(item => item.ID == currentUser.UserID, cancellationToken);
        if (user is null)
        {
            response.lstError.Add("User not found.");
            return response;
        }

        var previousUrl = user.CvUrl;
        user.CvUrl = null;
        await context.SaveChangesAsync(cancellationToken);
        await cloudinaryAssets.DeleteCvByUrlAsync(previousUrl, CancellationToken.None);
        return response;
    }
}
