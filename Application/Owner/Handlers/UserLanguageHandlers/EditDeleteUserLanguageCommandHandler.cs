using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class EditDeleteUserLanguageCommandHandler : IRequestHandler<EditDeleteUserLanguageCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;

        public EditDeleteUserLanguageCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<CommandResponse> Handle(EditDeleteUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.LstLanguages == null)
            {
                response.lstError.Add("Language list can't be null.");
                return response;
            }


            var existingEntity = await _context.User
                .Include(y => y.LstUserLanguages)
                .FirstOrDefaultAsync(u => u.ID == _currentUser.UserID!.Value, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("User not found.");
                return response;
            }

            var RequestedLanguages = request.LstLanguages.Select(x => x.LKP_LanguageID).ToList();

            var LKP_LanguageIDs = await _context.LKP_Language
                .AsNoTracking()
                .Where(l => RequestedLanguages.Contains(l.ID))
                .Select(l => l.ID)
                .ToListAsync(cancellationToken);

            if (RequestedLanguages.Count != LKP_LanguageIDs.Count)
            {
                response.lstError.Add("Wrong Language Entry.");
                return response;
            }

            var requestedProficiencies = request.LstLanguages
                .Select(language => language.LKP_LanguageProficiencyID)
                .Distinct()
                .ToList();
            var proficiencyCount = await _context.LKP_LanguageProficiency.CountAsync(
                proficiency => requestedProficiencies.Contains(proficiency.ID),
                cancellationToken);
            if (proficiencyCount != requestedProficiencies.Count)
            {
                response.lstError.Add("Wrong language proficiency entry.");
                return response;
            }

            var requestedByLanguage = request.LstLanguages.ToDictionary(
                language => language.LKP_LanguageID);
            var retainedLanguageIds = requestedByLanguage.Keys.ToHashSet();

            var removed = existingEntity.LstUserLanguages
                .Where(language => !retainedLanguageIds.Contains(language.LKP_LanguageID))
                .ToArray();
            _context.UserLanguage.RemoveRange(removed);

            foreach (var existingLanguage in existingEntity.LstUserLanguages.Except(removed))
            {
                existingLanguage.LKP_LanguageProficiencyID =
                    requestedByLanguage[existingLanguage.LKP_LanguageID].LKP_LanguageProficiencyID;
                requestedByLanguage.Remove(existingLanguage.LKP_LanguageID);
            }

            existingEntity.LstUserLanguages.AddRange(requestedByLanguage.Values.Select(language =>
                new Domain.Entities.UserLanguage
                {
                    UserID = existingEntity.ID,
                    LKP_LanguageID = language.LKP_LanguageID,
                    LKP_LanguageProficiencyID = language.LKP_LanguageProficiencyID
                }));

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
