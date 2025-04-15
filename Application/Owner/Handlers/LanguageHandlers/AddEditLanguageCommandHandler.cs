using Application.Common.Entities;
using Application.Common.Services;
using Application.Owner.Commands.LanguageCommands;
using Application.Owner.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.LanguageHandlers
{
    public class AddEditLanguageCommandHandler : IRequestHandler<AddEditLanguageCommand, AbstractViewModel>
    {
        private readonly CurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLanguageCommandHandler(IAppDbContext context, CurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            if (request.ID == null)
            {
                var ResultToDB = _mapper.Map<Language>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                await _context.Language.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldLanguage = await _context.Language
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldLanguage == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Language not found.");
                    return Vm;
                }

                _mapper.Map(request, oldLanguage);
                oldLanguage.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the language.");
            }

            return Vm;
        }
    }
}
