using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.CertificaeCommands
{
    public class SortCertificateCommand : IRequest<CommandResponse>
    {
        public List<Guid> CertificateIdsInOrder { get; set; }
    }
}
