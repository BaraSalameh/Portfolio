using Application.Common.Entities;
using Application.CV.Commands.EducationCommands;
using Application.CV.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CV.Handlers.EducationHandlers
{
    public class AddEditEducationCommandHandler : IRequestHandler<AddEditEducationCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditEducationCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new EducationMappingProfiles().AddEditEducationCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(AddEditEducationCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<Education>(request);

            if (request.ID == null)
            {
                _context.Education.Add(ResultToDB);
            }
            else
            {
                var oldEducation =
                    await _context.Education
                        .FindAsync(request.ID);

                if (oldEducation == null)
                {
                    Vm.lstError.Add("Education not found");
                    Vm.status = false;
                    return Vm;
                }

                _mapper.Map(request, oldEducation);
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Education");
            }

            return Vm;
        }
    }
}
