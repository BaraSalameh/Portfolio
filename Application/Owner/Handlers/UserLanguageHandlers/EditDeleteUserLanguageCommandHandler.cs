using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class EditDeleteUserLanguageCommandHandler : IRequestHandler<EditDeleteUserLanguageCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EditDeleteUserLanguageCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(EditDeleteUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            if (request.LstLanguages == null)
            {
                response.lstError.Add("Language list can't be null.");
                return response;
            }

            try
            {

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

                _mapper.Map(request, existingEntity);

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while editting/deleting the Language.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred");
            }

            return response;
        }
    }
}
