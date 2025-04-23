using Application.Account.Queries;
using Application.Common.Functions;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Application.Account.Handlers
{
    class LoginQueryHandler : IRequestHandler<LoginQuery, LQ_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoginQueryHandler(IAppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

            var claims = new List<Claim>
            {
                new (ClaimTypes.NameIdentifier, user.ID.ToString()!),
                new (ClaimTypes.Name, user.Username!),
                new (ClaimTypes.Role, user.Role.Name!)
            };

            var secretKey = _configuration["ApplicationSettings:JWT_Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var token = tokenHandler.WriteToken(securityToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true, // Make the cookie inaccessible to JavaScript
                Secure = true, // Ensure the cookie is sent over HTTPS only
                SameSite = SameSiteMode.None, // Prevent CSRF attacks
                Expires = DateTime.UtcNow.AddDays(1) // Expiration time
            };

            _httpContextAccessor.HttpContext!.Response.Cookies.Append("AuthToken", token, cookieOptions);

            Vm.Username = user.Username!;
            Vm.status = true;

            return Vm;
        }
    }
}