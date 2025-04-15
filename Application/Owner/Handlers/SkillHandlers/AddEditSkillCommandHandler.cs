using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.SkillCommands;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.SkillHandlers
{
    public class AddEditSkillCommandHandler : IRequestHandler<AddEditSkillCommand, AbstractViewModel>
    {
        private readonly ICurrentUserService _currentUser;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditSkillCommandHandler(IAppDbContext context, ICurrentUserService currentUser, IMapper mapper)
        {
            _context = context;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        public async Task<AbstractViewModel> Handle(AddEditSkillCommand request, CancellationToken cancellationToken)
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
                var ResultToDB = _mapper.Map<Skill>(request);
                ResultToDB.UserID = _currentUser.UserID.Value;
                await _context.Skill.AddAsync(ResultToDB, cancellationToken);
            }
            else
            {
                var oldSkill = await _context.Skill
                    .Where(x => x.UserID == _currentUser.UserID.Value && x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                    .FirstOrDefaultAsync();

                if (oldSkill == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Skill not found.");
                    return Vm;
                }

                _mapper.Map(request, oldSkill);
                oldSkill.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync(cancellationToken);
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while saving the Skill.");
            }

            return Vm;
        }
    }
}
