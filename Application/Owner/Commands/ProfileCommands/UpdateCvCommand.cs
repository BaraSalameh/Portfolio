using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.Profile;

public sealed class UpdateCvCommand : IRequest<CommandResponse<UpdateCvResponse>>
{
    public required byte[] Content { get; init; }
    public required string FileName { get; init; }
}

public sealed record UpdateCvResponse(string Url);

public sealed class RemoveCvCommand : IRequest<CommandResponse>;
