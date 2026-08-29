using Application.Common.Entities;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace Application.Owner.Commands.CertificaeCommands
{
    public class SortCertificateCommand : IRequest<CommandResponse>
    {
        [MaxLength(500)]
        public List<Guid> CertificateIdsInOrder { get; set; } = [];
    }
}
