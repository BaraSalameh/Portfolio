using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.Profile;

public sealed class RemoveProfileImageCommand : IRequest<CommandResponse>
{
    public required ProfileImageKind ImageKind { get; init; }
}
