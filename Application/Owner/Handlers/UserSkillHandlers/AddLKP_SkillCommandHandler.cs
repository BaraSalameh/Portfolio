using System.Text.RegularExpressions;
using Application.Common.Entities;
using Application.Common.Persistence;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserSkillCommands;
using Application.Owner.Queries.UserSkillQueries;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserSkillHandlers;

public sealed partial class AddLKP_SkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
    : IRequestHandler<AddLKP_SkillCommand, CommandResponse<LKP_SLQ_Response>>
{
    public async Task<CommandResponse<LKP_SLQ_Response>> Handle(AddLKP_SkillCommand request, CancellationToken cancellationToken)
    {
        var response = new CommandResponse<LKP_SLQ_Response>();
        _ = currentUser.UserID;
        var name = Whitespace().Replace(request.Name.Trim(), " ");
        var normalizedName = name.ToLower();
        var existing = await context.LKP_Skill
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

        var skill = new LKP_Skill { Name = name, Source = "Custom", IsActive = true };
        await context.LKP_Skill.AddAsync(skill, cancellationToken);
        await context.SaveChangesAsync(cancellationToken);
        response.Data = ToResponse(skill);
        return response;
    }

    private static LKP_SLQ_Response ToResponse(LKP_Skill skill) => new()
    {
        ID = skill.ID,
        Name = skill.Name,
        IconUrl = skill.IconUrl,
        Source = skill.Source
    };

    [GeneratedRegex(@"\s+")]
    private static partial Regex Whitespace();
}
