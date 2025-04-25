using Application.Client.Commands;
using Application.Common.Entities;
using Application.Common.Services.Interface;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Client.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserResolverService _userResolver;

        public SendEmailCommandHandler(IAppDbContext context, IMapper mapper, IUserResolverService userResolver)
        {
            _context = context;
            _mapper = mapper;
            _userResolver = userResolver;

        }
        public async Task<AbstractViewModel> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var user = await _userResolver.GetUserByEmailAsync(request.EmailTo, cancellationToken);

            if (user == null || user.ID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Error while finding the user");
                return Vm;
            }

            var ResultToDB = _mapper.Map<ContactMessage>(request);
            ResultToDB.UserID = user.ID.Value;

            await _context.ContactMessage.AddAsync(ResultToDB);

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while sending Email");
            }

            return Vm;
        }
    }
}
