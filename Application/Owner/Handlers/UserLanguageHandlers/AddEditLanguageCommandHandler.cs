using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using AutoMapper;
using DataAccess.DbContexts;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;


namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class AddEditUserLanguageCommandHandler : IRequestHandler<AddEditUserLanguageCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditUserLanguageCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            if (request.OldLKP_LanguageID == null)
            {
                var ResultToDB = _mapper.Map<UserLanguage>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                try
                {
                    await _context.UserLanguage.AddAsync(ResultToDB, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
                catch
                {
                    Vm.status = false;
                    Vm.lstError.Add("UserLanguage already exists");
                    return Vm;
                }
                
            }
            else
            {
                var appContext = (AppDbContext)_context;
                using var transaction = await appContext.Database.BeginTransactionAsync();

                try
                {
                    var oldUserLanguage = await _context.UserLanguage
                        .Where(x => 
                            x.UserID == _currentUser.UserID.Value && 
                            x.LKP_LanguageID == request.OldLKP_LanguageID && 
                            (x.IsDeleted == false || x.IsDeleted == null)
                        )
                        .FirstOrDefaultAsync(cancellationToken);

                    if(oldUserLanguage == null)
                    {
                        Vm.status = false;
                        Vm.lstError.Add("UserLanguage not found");
                        return Vm;
                    }
                    else
                    {
                        _context.UserLanguage.Remove(oldUserLanguage);
                    }

                    var oldUserLanguageCreatedAt = oldUserLanguage.CreatedAt;
                    await _context.SaveChangesAsync(cancellationToken);

                    var newUserLang = new UserLanguage
                    {
                        UserID = _currentUser.UserID.Value,
                        LKP_LanguageID = request.LKP_LanguageID,
                        LKP_LanguageProficiencyID = request.LKP_LanguageProficiencyID,
                        CreatedAt = oldUserLanguageCreatedAt,
                        UpdatedAt = DateTime.UtcNow
                    };

                    await _context.UserLanguage.AddAsync(newUserLang, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);


                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    Vm.status = false;
                    Vm.lstError.Add(ex.InnerException!.Message);
                    Vm.lstError.Add("Error while Updating the UserLanguage.");
                } 
            }

            return Vm;
        }
    }
}
