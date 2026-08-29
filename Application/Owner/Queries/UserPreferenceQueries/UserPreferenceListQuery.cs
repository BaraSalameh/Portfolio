using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserPreferenceQueries
{
    public class UserPreferenceListQuery : OwnerCollectionQuery<UPLQ_Response> { }

    public class UPLQ_Response
    {
        public UPLQ_LKP_Preference Preference { get; set; } = null!;
        public string Value { get; set; } = string.Empty;
    }

    public class UPLQ_LKP_Preference
    {
        public Guid ID { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
