using Application.Account.Commands;
using Application.Account.MappingProfiles;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new RegisterMappingProfiles().RegisterCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var Output = new AbstractViewModel();
            request.Password = request.Password!.Encrypt(true);
            var ResultToDB = _mapper.Map<User>(request);

            try
            {
                _context.User.Add(ResultToDB);
                await _context.SaveChangesAsync();
                Output.status = true;
            }
            catch
            {
                Output.lstError.Add("Error while registering");
            }

            return Output;
        }
    }
}