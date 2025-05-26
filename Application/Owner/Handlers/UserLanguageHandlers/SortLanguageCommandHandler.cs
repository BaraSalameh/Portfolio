using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using Application.Owner.Commands.UserLanguageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class SortLanguageCommandHandler : IRequestHandler<SortLanguageCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public SortLanguageCommandHandler(IAppDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CommandResponse> Handle(SortLanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var languages = await _context.UserLanguage
                    .Where(e => e.UserID == _currentUserService.UserID && request.LanguageIdsInOrder.Contains(e.LKP_LanguageID))
                    .ToListAsync();

                if (languages.Count != request.LanguageIdsInOrder.Count)
                {
                    response.lstError.Add("Mismatch between IDs in the request and database records.");
                    return response;
                }

                for (int i = 0; i < request.LanguageIdsInOrder.Count; i++)
                {
                    var id = request.LanguageIdsInOrder[i];
                    var lan = languages.First(e => e.LKP_LanguageID == id);
                    lan.Order = i + 1;
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
