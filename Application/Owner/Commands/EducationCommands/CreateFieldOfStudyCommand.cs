using System.ComponentModel.DataAnnotations;
using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using MediatR;

namespace Application.Owner.Commands.EducationCommands;

public sealed class CreateFieldOfStudyCommand : IRequest<CommandResponse<LKP_FOSLQ_Response>>
{
    [Required, StringLength(100, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
