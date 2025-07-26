using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.CertificaeCommands
{
    public class AddEditDeleteCertificateCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        public Guid LKP_CertificateID { get; set; }
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public List<Guid>? LstSkills { get; set; }
        public List<string> LstCertificateMedias { get; set; }
    }
}
