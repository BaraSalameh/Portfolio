using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_LanguageCommands
{
    public class DeleteLKP_LanguageCommand : IRequest<CommandResponse>
    {
        public Guid ID { get; set; }
    }
}
