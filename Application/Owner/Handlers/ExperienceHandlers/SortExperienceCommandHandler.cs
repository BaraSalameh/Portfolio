using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using Application.Common.Persistence;
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
            if (request.ExperienceIdsInOrder.Count != request.ExperienceIdsInOrder.Distinct().Count())
            {
                response.lstError.Add("Duplicate experience IDs are not allowed.");
                return response;
            }

            var experiences = await _context.Experience
                .Where(entity => entity.UserID == _currentUserService.UserID
                    && !entity.IsDeleted)
                .OrderBy(entity => entity.ID)
                .Take(501)
                .ToDictionaryAsync(entity => entity.ID, cancellationToken);

            if (experiences.Count > 500 || !experiences.Keys.ToHashSet().SetEquals(request.ExperienceIdsInOrder))
            {
                response.lstError.Add("The request must contain every active experience exactly once.");
                return response;
            }

            for (int i = 0; i < request.ExperienceIdsInOrder.Count; i++)
            {
                experiences[request.ExperienceIdsInOrder[i]].Order = i + 1;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
