using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.UserLanguageCommands
{
    public class SortLanguageCommand : IRequest<CommandResponse>
    {
        public List<Guid> LanguageIdsInOrder { get; set; }
    }
}
