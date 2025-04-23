using Application.Account.Commands;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            request.Password = request.Password!.Encrypt(true);
            var baseUserName = $"{request.Firstname}-{request.Lastname}".ToLower().Replace(" ", "-");
            var guidSuffix = Guid.NewGuid().ToString("N").Substring(0, 6);

            var ResultToDB = _mapper.Map<User>(request);
            ResultToDB.Username = $"{baseUserName}-{guidSuffix}";
            ResultToDB.RoleID =  RoleIdentifiers.Owner;
            ResultToDB.CreatedAt = DateTime.UtcNow;

            try
            {
                await _context.User.AddAsync(ResultToDB, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Email/Username doesint exist!");
            }

            return Vm;
        }
    }
}