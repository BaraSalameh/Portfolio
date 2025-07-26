using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.CertificaeCommands
{
    public class DeleteCertificateCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
