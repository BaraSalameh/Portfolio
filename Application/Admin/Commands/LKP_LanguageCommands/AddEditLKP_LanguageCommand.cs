using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_LanguageCommands
{
    public class AddEditLKP_LanguageCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        public string Name { get; set; }
    }
}
