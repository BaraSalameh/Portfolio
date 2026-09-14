using System.ComponentModel.DataAnnotations;
using Application.Common.Entities;
using Application.Owner.Queries.UserSkillQueries;
using MediatR;

namespace Application.Owner.Commands.UserSkillCommands;

public sealed class AddLKP_SkillCommand : IRequest<CommandResponse<LKP_SLQ_Response>>
{
    [Required, StringLength(120, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
