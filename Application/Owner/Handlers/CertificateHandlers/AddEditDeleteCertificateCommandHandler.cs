using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.CertificaeCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.CertificateHandlers
{
    public class AddEditDeleteCertificateCommandHandler : IRequestHandler<AddEditDeleteCertificateCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditDeleteCertificateCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditDeleteCertificateCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var validIds = await _context.LKP_Technology
                    .Where(t => (request.LstSkills ?? new List<Guid>()).Contains(t.ID))
                    .Select(t => t.ID)
                    .ToListAsync(cancellationToken);

                if (validIds.Count != (request.LstSkills ?? []).Count)
                {
                    response.lstError.Add("Some Skills do not exist.");
                    return response;
                }

                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<Certificate>(request);
                    newEntity.UserID = _currentUser.UserID!.Value;

                    await _context.Certificate.AddAsync(newEntity, cancellationToken);
                }
                else
                {
                    var existingEntity = await _context.Certificate
                        .Include(x => x.LstUserSkills)
                        .FirstOrDefaultAsync(x =>
                            x.UserID == _currentUser.UserID!.Value &&
                            x.ID == request.ID &&
                            x.IsDeleted == false,
                            cancellationToken
                        );

                    if (existingEntity == null)
                    {
                        response.lstError.Add("Skill not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating/deleting the Certificate.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred");
            }

            return response;
        }
    }
}
