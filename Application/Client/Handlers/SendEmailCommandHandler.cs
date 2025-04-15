using Application.Client.Commands;
using Application.Client.MappingProfiles;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class SendEmailCommandHandler : IRequestHandler<SendEmailCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public SendEmailCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

        }
        public async Task<AbstractViewModel> Handle(SendEmailCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            var user = await GetUserByEmailAsync(request.EmailTo);

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

        private async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.User
                .Where(u => u.Email == email)
                .FirstOrDefaultAsync();
        }
    }
}
