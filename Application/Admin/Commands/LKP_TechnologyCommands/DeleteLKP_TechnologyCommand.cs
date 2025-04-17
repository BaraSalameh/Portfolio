using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_TechnologyCommands
{
    public class DeleteLKP_TechnologyCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
