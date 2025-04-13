using Application.Account.Queries.LoginQueries;
using Application.Common.Functions;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Account.Handlers.LoginHandlers
{
    class LoginQueryHandler : IRequestHandler<LoginQuery, LQ_Response>
    {
        private readonly IAppDbContext _context;

        public LoginQueryHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<LQ_Response> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var Vm = new LQ_Response();
            string EncryptedPassword = request.Password.Encrypt(true);
            var user =
                 await _context.User
                    .Where(u => u.Email == request.Email && u.Password == EncryptedPassword && u.IsActive == true)
                    .Include(u => u.Role)
                    .FirstOrDefaultAsync();

            if (user == null)
            {
                Vm.lstError.Add("Wrong username or password");
                Vm.status = false;
                return Vm;
            }

            var lstRoleClaims = new List<Claim>()
            {
                new ("ID", user.ID.ToString()!),
                new ("Email", user.Email!),
                new ("Firstname", user.Firstname!),
                new ("Lastname", user.Lastname!),
                new (ClaimTypes.Role, user.Role.Name!)
            };

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(lstRoleClaims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes("vFZJiZwVznnX3Pr65dZV9IsI0NtbvVasSG4kPRvn2p4=")),
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            Vm.status = true;
            Vm.token = tokenHandler.WriteToken(securityToken);
            Vm.Firstname = user.Firstname;
            Vm.Lastname = user.Lastname;

            return Vm;
        }
    }
}