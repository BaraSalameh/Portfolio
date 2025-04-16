using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_LanguageProficiencyCommands
{
    public class DeleteLKP_LanguageProficiencyCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
