using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.LanguageCommands
{
    public class AddEditLanguageCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? name { get; set; }
        public int? Proficiency { get; set; }
    }
}
