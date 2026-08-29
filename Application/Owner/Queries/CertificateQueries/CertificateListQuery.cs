using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.CertificateQueries
{
    public class CertificateListQuery : OwnerCollectionQuery<CLQ_Response> { }

    public class CLQ_Response
    {
        public Guid ID { get; set; }
        public CLQ_LKP_Certificate Certificate { get; set; } = null!;
        public DateOnly? IssueDate { get; set; }
        public DateOnly? ExpirationDate { get; set; }
        public string? CredintialID { get; set; }
        public string? CredintialUrl { get; set; }
        public List<CLQ_Skill> LstSkills { get; set; } = [];
        public List<CLQ_CertificateMedia> LstCertificateMedias { get; set; } = [];
    }

    public class CLQ_LKP_Certificate
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class CLQ_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string IconUrl { get; set; } = string.Empty;
    }

    public class CLQ_CertificateMedia
    {
        public Guid ID { get; set; }
        public string Url { get; set; } = string.Empty;
    }
}
