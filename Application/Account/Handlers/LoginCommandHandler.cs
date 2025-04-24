using Application.Account.Commands;
using Application.Common.Functions;
using Application.Common.Services.Interface;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Account.Handlers
{
    class LoginCommandHandler : IRequestHandler<LoginCommand, LC_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IAuthService _authService;

        public LoginCommandHandler(IAppDbContext context, ITokenService tokenService, ICookieService cookieService, IAuthService authService)
        {
            _context = context;
            _authService = authService;
        }

        public async Task<LC_Response> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var Vm = new LC_Response();
            string EncryptedPassword = request.Password.Encrypt(true);
            var user =
                 await _context.User
                    .Where(u => u.Email == request.Email && u.Password == EncryptedPassword)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                Vm.lstError.Add("Wrong username or password");
                Vm.status = false;
                return Vm;
            }

            _authService.AuthSetupAsync(user, request.RememberMe);
            await _context.SaveChangesAsync(cancellationToken);

            Vm.Username = user.Username!;
            Vm.Role = user.Role.Name!;
            Vm.status = true;

            return Vm;
        }
    }
} 