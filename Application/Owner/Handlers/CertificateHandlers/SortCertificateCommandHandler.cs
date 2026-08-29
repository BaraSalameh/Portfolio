using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using Application.Common.Persistence;
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
            if (request.CertificateIdsInOrder.Count != request.CertificateIdsInOrder.Distinct().Count())
            {
                response.lstError.Add("Duplicate certificate IDs are not allowed.");
                return response;
            }

            var certificates = await _context.Certificate
                .Where(entity => entity.UserID == _currentUserService.UserID
                    && !entity.IsDeleted)
                .OrderBy(entity => entity.ID)
                .Take(501)
                .ToDictionaryAsync(entity => entity.ID, cancellationToken);

            if (certificates.Count > 500 || !certificates.Keys.ToHashSet().SetEquals(request.CertificateIdsInOrder))
            {
                response.lstError.Add("The request must contain every active certificate exactly once.");
                return response;
            }

            for (int i = 0; i < request.CertificateIdsInOrder.Count; i++)
            {
                certificates[request.CertificateIdsInOrder[i]].Order = i + 1;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
