using Application.Account.Commands;
using Application.Common.Constants;
using Application.Common.Functions;
using Application.Common.Services.Interface;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using Domain.Enums;
using MediatR;

namespace Application.Account.Handlers
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, RC_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly IUserNotificationService _userNotificationService;

        public RegisterCommandHandler(IAppDbContext context, IMapper mapper, IDateTimeProvider dateTimeProvider, IUserNotificationService userNotificationService)
        {
            _context = context;
            _mapper = mapper;
            _dateTimeProvider = dateTimeProvider;
            _userNotificationService = userNotificationService;
        }

        public async Task<RC_Response> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var Vm = new RC_Response();

            request.Password = request.Password!.Encrypt(true);
            var baseUserName = $"{request.Firstname}-{request.Lastname}".ToLower().Replace(" ", "-");
            var guidSuffix = Guid.NewGuid().ToString("N").Substring(0, 6);

            var confirmationToken = Guid.NewGuid().ToString();
            var pendingEmail = _mapper.Map<PendingEmailConfirmation>(request);
            pendingEmail.Token = confirmationToken;
            pendingEmail.ExpiresAt = _dateTimeProvider.UtcNow.Add(ExpirationTimes.PendingEmailTokenLifeTime);

            var ResultToDB = _mapper.Map<User>(request);
            ResultToDB.Username = $"{baseUserName}-{guidSuffix}";
            ResultToDB.RoleID =  RoleIdentifiers.Owner;
            ResultToDB.CreatedAt = _dateTimeProvider.UtcNow;
            ResultToDB.LstPendingEmailConfirmations.Add(pendingEmail);

            var role = await _context.Role.FindAsync(RoleIdentifiers.Owner, cancellationToken);
            if (role == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Error while assigning role!");
                return Vm;
            }

            ResultToDB.Role = role;

            try
            {
                await _context.User.AddAsync(ResultToDB, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                Vm.status = true;
                Vm.Username = ResultToDB.Username;
                Vm.Role = ResultToDB.Role!.Name!;

                Console.WriteLine("I am in Register");
                await _userNotificationService.SendEmailConfirmationAsync(ResultToDB);
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Email/Username already exist!");
            }

            return Vm;
        }
    }
}