using Application.Account.Queries.LoginQueries;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers.LoginHandlers
{
    class LoginQueryHandler : IRequestHandler<LoginQuery, AbstractViewModel>
    {
        private readonly IAppDbContext _context;

        public LoginQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<AbstractViewModel> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            string EncryptedPassword = request.Password.Encrypt(true);
            var user =
                 await _context.User
                    .Where(u => u.Email == request.Email && u.Password == EncryptedPassword && u.IsActive == true)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                Vm.lstError.Add("Wrong username or password");
                Vm.status = false;
                return Vm;
            }

            return Vm;
        }
    }
}