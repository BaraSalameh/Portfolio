using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectHandlers
{
    public class SortProjectCommandHandler : IRequestHandler<SortProjectCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SortProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResponse> Handle(SortProjectCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            if (request.ProjectIdsInOrder.Count != request.ProjectIdsInOrder.Distinct().Count())
            {
                response.lstError.Add("Duplicate project IDs are not allowed.");
                return response;
            }

            var projects = await _context.Project
                .Where(entity => entity.UserID == _currentUserService.UserID
                    && !entity.IsDeleted)
                .OrderBy(entity => entity.ID)
                .Take(501)
                .ToDictionaryAsync(entity => entity.ID, cancellationToken);

            if (projects.Count > 500 || !projects.Keys.ToHashSet().SetEquals(request.ProjectIdsInOrder))
            {
                response.lstError.Add("The request must contain every active project exactly once.");
                return response;
            }

            for (int i = 0; i < request.ProjectIdsInOrder.Count; i++)
            {
                projects[request.ProjectIdsInOrder[i]].Order = i + 1;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
