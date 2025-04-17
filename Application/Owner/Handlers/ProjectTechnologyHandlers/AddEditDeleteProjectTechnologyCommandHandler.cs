using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectTechnologyCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectTechnologyHandlers
{
    public class AddEditDeleteProjectTechnologyCommandHandler : IRequestHandler<AddEditDeleteProjectTechnologyCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditDeleteProjectTechnologyCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditDeleteProjectTechnologyCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();

            if (!_currentUser.IsAuthenticated || _currentUser.UserID == null)
            {
                Vm.status = false;
                Vm.lstError.Add("Unauthorized user.");
                return Vm;
            }

            var validIds = await _context.LKP_Technology
                .Where(t => (request.LstProjectTechnologies ?? new List<Guid>()).Contains(t.ID))
                .Select(t => t.ID)
                .ToListAsync(cancellationToken);

            if (validIds.Count != (request.LstProjectTechnologies ?? []).Count)
            {
                Vm.status = false;
                Vm.lstError.Add("Some technologies do not exist.");
                return Vm;
            }

            if (request.ID == null)
            {
                var ResultToDB = _mapper.Map<Project>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;

                await _context.Project.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldProjectTechnology = await _context.Project
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .Include(x => x.LstProjectTechnologies)
                    .FirstOrDefaultAsync();

                if (oldProjectTechnology == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Project or Technology not found.");
                    return Vm;
                }


                _mapper.Map(request, oldProjectTechnology);
                oldProjectTechnology.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the ProjectTechnology.");
            }

            return Vm;
        }
    }
}
