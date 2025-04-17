using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class EditDeleteUserLanguageCommandHandler : IRequestHandler<EditDeleteUserLanguageCommand, AbstractViewModel>
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

        public async Task<AbstractViewModel> Handle(EditDeleteUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            if(request.LstLanguages != null)
            {

                var oldUser = await _context.User
                    .Where(u => u.ID == _currentUser.UserID.Value && u.IsActive == true)
                    .Include(y => y.LstUserLanguages)
                    .FirstOrDefaultAsync(cancellationToken);

                if (oldUser == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("User not found.");
                    return Vm;
                }

                var RequestedLanguages = request.LstLanguages.Select(x => x.LKP_LanguageID).ToList();
                var RequestedLanguageProficiencies = request.LstLanguages.Select(x => x.LKP_LanguageProficiencyID).ToList();

                var LKP_LanguageIDs = await _context.LKP_Language
                    .Where(l => RequestedLanguages.Contains(l.ID))
                    .Select(l => l.ID)
                    .ToListAsync(cancellationToken);

                if (RequestedLanguages.Count != LKP_LanguageIDs.Count)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Wrong Language Entry.");
                    return Vm;
                }

                _mapper.Map(request, oldUser);
            }
            else
            {
                Vm.status = false;
                Vm.lstError.Add("There Should be some languages to add.");
                return Vm;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the Language.");
            }

            return Vm;
        }
    }
}
