using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ExperienceCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ExperienceHandlers
{
    public class AddEditExperienceCommandHandler : IRequestHandler<AddEditExperienceCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditExperienceCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditExperienceCommand request, CancellationToken cancellationToken)
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
                var ResultToDB = _mapper.Map<Experience>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                await _context.Experience.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldExperience = await _context.Experience
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldExperience == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Experience not found.");
                    return Vm;
                }

                _mapper.Map(request, oldExperience);
                oldExperience.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the Experience.");
            }

            return Vm;
        }
    }
}
