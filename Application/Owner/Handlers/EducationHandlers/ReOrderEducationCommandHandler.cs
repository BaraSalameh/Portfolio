using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class ReOrderEducationCommandHandler : IRequestHandler<ReOrderEducationCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public ReOrderEducationCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResponse> Handle(ReOrderEducationCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var educations = await _context.Education
                    .Where(e => e.UserID == _currentUserService.UserID && request.EducationIdsInOrder.Contains(e.ID))
                    .ToListAsync();

                if (educations.Count != request.EducationIdsInOrder.Count)
                {
                    response.lstError.Add("Mismatch between IDs in the request and database records.");
                    return response;
                }

                for (int i = 0; i < request.EducationIdsInOrder.Count; i++)
                {
                    var id = request.EducationIdsInOrder[i];
                    var edu = educations.First(e => e.ID == id);
                    edu.Order = i + 1;
                }

                await _context.SaveChangesAsync();
            }
            catch(DbUpdateException dbEx)
            {
                response.lstError.Add("Error while re ordering education.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
