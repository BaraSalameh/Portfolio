using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers
{
    public class SortCertificateCommandHandler : IRequestHandler<SortCertificateCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SortCertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResponse> Handle(SortCertificateCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var Certificates = await _context.Certificate
                    .Where(e => e.UserID == _currentUserService.UserID && request.CertificateIdsInOrder.Contains(e.ID))
                    .ToListAsync();

                if (Certificates.Count != request.CertificateIdsInOrder.Count)
                {
                    response.lstError.Add("Mismatch between IDs in the request and database records.");
                    return response;
                }

                for (int i = 0; i < request.CertificateIdsInOrder.Count; i++)
                {
                    var id = request.CertificateIdsInOrder[i];
                    var edu = Certificates.First(e => e.ID == id);
                    edu.Order = i + 1;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while re ordering Certificate.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
