using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.LanguageCommands
{
    public class AddEditLanguageCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? name { get; set; }
        public int LanguageLevelID { get; set; }
        public int ProfileID { get; set; }
    }
}
