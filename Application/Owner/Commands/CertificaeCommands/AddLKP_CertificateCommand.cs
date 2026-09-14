using System.ComponentModel.DataAnnotations;
using Application.Common.Entities;
using Application.Owner.Queries.CertificateQueries;
using MediatR;

namespace Application.Owner.Commands.CertificaeCommands;

public sealed class AddLKP_CertificateCommand : IRequest<CommandResponse<LKP_CLQ_Response>>
{
    [Required, StringLength(160, MinimumLength = 2)]
    public string Name { get; set; } = string.Empty;
}
