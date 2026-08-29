using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.EducationCommands;
using Application.Common.Persistence;
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
            if (request.EducationIdsInOrder.Count != request.EducationIdsInOrder.Distinct().Count())
            {
                response.lstError.Add("Duplicate education IDs are not allowed.");
                return response;
            }

            var educations = await _context.Education
                .Where(entity => entity.UserID == _currentUserService.UserID
                    && !entity.IsDeleted)
                .OrderBy(entity => entity.ID)
                .Take(501)
                .ToDictionaryAsync(entity => entity.ID, cancellationToken);

            if (educations.Count > 500 || !educations.Keys.ToHashSet().SetEquals(request.EducationIdsInOrder))
            {
                response.lstError.Add("The request must contain every active education record exactly once.");
                return response;
            }

            for (int i = 0; i < request.EducationIdsInOrder.Count; i++)
            {
                educations[request.EducationIdsInOrder[i]].Order = i + 1;
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
