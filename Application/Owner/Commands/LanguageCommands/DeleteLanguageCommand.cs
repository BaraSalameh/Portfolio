using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.LanguageCommands
{
    public class DeleteLanguageCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
