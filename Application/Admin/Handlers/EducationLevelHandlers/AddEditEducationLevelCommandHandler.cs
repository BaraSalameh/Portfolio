using Application.Admin.Commands.EducationLevelCommands;
using Application.Admin.MappingProfiles;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.EducationLevelHandlers
{
    class AddEditEducationLevelCommandHandler : IRequestHandler<AddEditEducationLevelCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditEducationLevelCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new EducationLevelMappingProfiles().AddEditEducationLevelCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(AddEditEducationLevelCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_EducationLevel>(request);

            if (request.ID == null)
            {
                _context.LKP_EducationLevel.Add(ResultToDB);
            }
            else
            {
                var oldEducationLevel =
                    await _context.LKP_EducationLevel
                        .Where(x => x.ID == request.ID && x.IsDeleted == false)
                        .FirstOrDefaultAsync();

                if (oldEducationLevel == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Education level not found");
                    return Vm;
                }

                _mapper.Map(request, oldEducationLevel);
                oldEducationLevel.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Education level");
            }

            return Vm;
        }
    }
}
