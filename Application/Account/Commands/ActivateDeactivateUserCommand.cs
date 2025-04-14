using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class ActivateDeactivateUserCommand : IRequest<AbstractViewModel>
    {
        public int ID { get; set; }
    }
}
