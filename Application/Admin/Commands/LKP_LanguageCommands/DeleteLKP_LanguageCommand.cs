using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_LanguageCommands
{
    public class DeleteLKP_LanguageCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
