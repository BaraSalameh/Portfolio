using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectTechnologyCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class AddEditDeleteProjectTechnologyCommandHandler : IRequestHandler<AddEditDeleteProjectTechnologyCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditDeleteProjectTechnologyCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditDeleteProjectTechnologyCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var validIds = await _context.LKP_Technology
                    .Where(t => (request.LstProjectTechnologies ?? new List<Guid>()).Contains(t.ID))
                    .Select(t => t.ID)
                    .ToListAsync(cancellationToken);

                if (validIds.Count != (request.LstProjectTechnologies ?? []).Count)
                {
                    response.lstError.Add("Some technologies do not exist.");
                    return response;
                }

                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<Project>(request);
                    newEntity.UserID = _currentUser.UserID!.Value;

                    await _context.Project.AddAsync(newEntity, cancellationToken);
                }
                else
                {
                    var existingEntity = await _context.Project
                        .Include(x => x.LstProjectTechnologies)
                        .FirstOrDefaultAsync(x =>
                            x.UserID == _currentUser.UserID!.Value &&
                            x.ID == request.ID &&
                            x.IsDeleted == false,
                            cancellationToken
                        );

                    if (existingEntity == null)
                    {
                        response.lstError.Add("Project or Technology not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating/deleting the ProjectTechnology.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred");
            }

            return response;
        }
    }
}
