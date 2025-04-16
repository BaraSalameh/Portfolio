using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.UserLanguageCommands;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Owner.Handlers.UserLanguageHandlers
{
    public class DeleteUserLanguageCommandHandler : IRequestHandler<DeleteUserLanguageCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly ICurrentUserService _currentUser;

        public DeleteUserLanguageCommandHandler(IAppDbContext context, ICurrentUserService currentUser)
        {
            _context = context;
            _currentUser = currentUser;
        }

        public async Task<AbstractViewModel> Handle(DeleteUserLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var oldUserLanguage = await _context.UserLanguage
                .Where(x =>
                    x.UserID == _currentUser.UserID.Value &&
                    x.LKP_LanguageID == request.LKP_LanguageID &&
                    (x.IsDeleted == false || x.IsDeleted == null)
                )
                .FirstOrDefaultAsync(cancellationToken);

            if(oldUserLanguage == null)
            {
                Vm.status = false;
                Vm.lstError.Add("UserLanguage not found");
                return Vm;
            }

            try
            {
                oldUserLanguage.IsDeleted = true;
                oldUserLanguage.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while Updating the UserLanguage.");
            }

            return Vm;
        }
    }
}
