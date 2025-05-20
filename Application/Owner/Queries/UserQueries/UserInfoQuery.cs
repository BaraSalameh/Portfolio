using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserQueries
{
    public class UserInfoQuery : IRequest<SingleQueryResponse<UIQ_Response>> { }

    public class UIQ_Response
    {
        public string? Username { get; set; }
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Title { get; set; }
        public string? Email { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public string? CoverPhoto { get; set; }
        public DateOnly? BirthDate { get; set; }
        public int? Gender { get; set; }
    }
}
