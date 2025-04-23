using Application.Common.Entities;
using MediatR;

namespace Application.Account.Queries
{
    public class LogoutQuery : IRequest<AbstractViewModel>
    {
    }
}
