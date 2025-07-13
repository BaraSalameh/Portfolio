using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserPreferenceQueries
{
    public class UserPreferenceListQuery : IRequest<ListQueryResponse<UPLQ_Response>> { }
    
    public class UPLQ_Response
    {
        public UPLQ_LKP_Preference Preference { get; set; }
        public string Value { get; set; }
    }

    public class UPLQ_LKP_Preference
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
