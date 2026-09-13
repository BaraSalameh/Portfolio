using System.Text.RegularExpressions;
using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using Application.Owner.Queries.EducationQueries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers;

public sealed partial class CreateFieldOfStudyCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<CreateFieldOfStudyCommand, CommandResponse<LKP_FOSLQ_Response>>
{
    public async Task<CommandResponse<LKP_FOSLQ_Response>> Handle(
        CreateFieldOfStudyCommand request,
        CancellationToken cancellationToken)
    {
        var response = new CommandResponse<LKP_FOSLQ_Response>();
        _ = currentUser.UserID;
        var name = Whitespace().Replace(request.Name.Trim(), " ");
        var normalizedName = name.ToLower();
        var existing = await context.LKP_FieldOfStudy
            .FirstOrDefaultAsync(item => item.Name.ToLower() == normalizedName, cancellationToken);

        if (existing is not null)
        {
            if (!existing.IsActive)
            {
                existing.IsActive = true;
                await context.SaveChangesAsync(cancellationToken);
            }

            response.Data = ToResponse(existing);
            return response;
        }

        var field = new LKP_FieldOfStudy
        {
            Name = name,
            Source = "Custom",
            IsActive = true
        };
        await context.LKP_FieldOfStudy.AddAsync(field, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        response.Data = ToResponse(field);
        return response;
    }

    private static LKP_FOSLQ_Response ToResponse(LKP_FieldOfStudy field) => new()
    {
        ID = field.ID,
        Name = field.Name,
        Source = field.Source
    };

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
