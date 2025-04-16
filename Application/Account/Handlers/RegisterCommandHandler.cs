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
            var ResultToDB = _mapper.Map<User>(request);
            ResultToDB.RoleID = (int)RoleName.Owner;
            ResultToDB.CreatedAt = DateTime.UtcNow;

            try
            {
                await _context.User.AddAsync(ResultToDB, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch (Exception ex)
            {
                Vm.status = false;
                Vm.lstError.Add("Error while registering or the email is already exists");
            }

            return Vm;
        }
    }
}