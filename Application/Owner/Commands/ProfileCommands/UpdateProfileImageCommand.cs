using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.Profile;

public enum ProfileImageKind
{
    ProfilePicture,
    CoverPhoto
}

public sealed class UpdateProfileImageCommand : IRequest<CommandResponse<UpdateProfileImageResponse>>
{
    public required byte[] Content { get; init; }
    public required ProfileImageKind ImageKind { get; init; }
}

public sealed record UpdateProfileImageResponse(string Url);
