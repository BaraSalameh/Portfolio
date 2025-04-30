using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.Profile
{
    public class EditProfileCommand : IRequest<CommandResponse>
    {
        public string? Firstname { get; set; }
        public string? Lastname { get; set; }
        public string? Title { get; set; }
        public string? Bio { get; set; }
        public string? Phone { get; set; }
        public string? ProfilePicture { get; set; }
        public int? Gender { get; set; }
        public DateOnly? BirthDate { get; set; }
    }
}
