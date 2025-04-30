using Application.Common.Entities;

namespace Application.Client.Queries
{
    public class UserListQuery : ListQuery<ULQ_Response> { }
    public class ULQ_Response
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Username { get; set; }
        public string? Title { get; set; }
        public string? ProfilePicture { get; set; }
    }
}
