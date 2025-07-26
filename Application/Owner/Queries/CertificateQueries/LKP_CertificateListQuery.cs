using Application.Common.Entities;

namespace Application.Owner.Queries.CertificateQueries
{
    public class LKP_CertificateListQuery : ListQuery<LKP_CLQ_Response> { }

    public class LKP_CLQ_Response
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
