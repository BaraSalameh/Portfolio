using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectCommands;
using DataAccess.Interfaces;
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

            try
            {
                var project = await _context.Project
                    .Where(e => e.UserID == _currentUserService.UserID && request.ProjectIdsInOrder.Contains(e.ID))
                    .ToListAsync();

                if (project.Count != request.ProjectIdsInOrder.Count)
                {
                    response.lstError.Add("Mismatch between IDs in the request and database records.");
                    return response;
                }

                for (int i = 0; i < request.ProjectIdsInOrder.Count; i++)
                {
                    var id = request.ProjectIdsInOrder[i];
                    var edu = project.First(e => e.ID == id);
                    edu.Order = i + 1;
                }

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
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
