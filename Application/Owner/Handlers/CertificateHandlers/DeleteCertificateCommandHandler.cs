using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers
{
    public class DeleteCertificateCommandHandler : IRequestHandler<DeleteCertificateCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public DeleteCertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _currentUser = currentUser;
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteCertificateCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.Certificate
                .Include(c => c.LstUserSkillCertificates)
                .FirstOrDefaultAsync(x =>
                    x.UserID == _currentUser.UserID!.Value &&
                    x.ID == request.ID &&
                    x.IsDeleted == false,
                    cancellationToken
                );

            if (existingEntity == null)
            {
                response.lstError.Add("Certificate not found.");
                return response;
            }

            existingEntity.IsDeleted = true;
            existingEntity.LstUserSkillCertificates.Clear();
            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
