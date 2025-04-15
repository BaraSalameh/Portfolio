using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.ProjectCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ProjectHandlers
{
    public class AddEditProjectCommandHandler : IRequestHandler<AddEditProjectCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditProjectCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditProjectCommand request, CancellationToken cancellationToken)
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
                var ResultToDB = _mapper.Map<Project>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;

                
                await _context.Project.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldProject = await _context.Project
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldProject == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Project not found.");
                    return Vm;
                }

                _mapper.Map(request, oldProject);
                oldProject.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the Project.");
            }

            return Vm;
        }
    }
}
