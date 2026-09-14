using System.Text.RegularExpressions;
using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using Application.Owner.Queries.CertificateQueries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers;

public sealed partial class AddLKP_CertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AddLKP_CertificateCommand, CommandResponse<LKP_CLQ_Response>>
{
    public async Task<CommandResponse<LKP_CLQ_Response>> Handle(AddLKP_CertificateCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<LKP_CLQ_Response>();
        _ = currentUser.UserID;
        var name = Whitespace().Replace(request.Name.Trim(), " ");
        var normalizedName = name.ToLower();
        var existing = await context.LKP_Certificate
            .FirstOrDefaultAsync(item => item.Name.ToLower() == normalizedName, cancellationToken);

        if (existing is not null)
        {
            response.Data = ToResponse(existing);
            return response;
        }

        var certificate = new LKP_Certificate { Name = name };
        await context.LKP_Certificate.AddAsync(certificate, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        response.Data = ToResponse(certificate);
        return response;
    }

    private static LKP_CLQ_Response ToResponse(LKP_Certificate certificate) => new()
    {
        ID = certificate.ID,
        Name = certificate.Name
    };

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
