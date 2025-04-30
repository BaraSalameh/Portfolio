using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class AddEditEducationCommandHandler : IRequestHandler<AddEditEducationCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditEducationCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditEducationCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<Education>(request);
                    newEntity.UserID = _currentUser.UserID!.Value;
                    await _context.Education.AddAsync(newEntity, cancellationToken);
                }
                else
                {
                    var existingEntity = await _context.Education
                        .FirstOrDefaultAsync(x =>
                            x.UserID == _currentUser.UserID!.Value &&
                            x.ID == request.ID &&
                            x.IsDeleted == false, 
                            cancellationToken
                        );

                    if (existingEntity == null)
                    {
                        response.lstError.Add("Education not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the Education.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
