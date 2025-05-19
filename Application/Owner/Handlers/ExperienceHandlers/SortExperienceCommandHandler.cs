using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class SortExperienceCommandHandler : IRequestHandler<SortExperienceCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SortExperienceCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResponse> Handle(SortExperienceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var experiences = await _context.Experience
                    .Where(e => e.UserID == _currentUserService.UserID && request.ExperienceIdsInOrder.Contains(e.ID))
                    .ToListAsync();

                if (experiences.Count != request.ExperienceIdsInOrder.Count)
                {
                    response.lstError.Add("Mismatch between IDs in the request and database records.");
                    return response;
                }

                for (int i = 0; i < request.ExperienceIdsInOrder.Count; i++)
                {
                    var id = request.ExperienceIdsInOrder[i];
                    var edu = experiences.First(e => e.ID == id);
                    edu.Order = i + 1;
                }

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException dbEx)
            {
                response.lstError.Add("Error while re sorting experience.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
