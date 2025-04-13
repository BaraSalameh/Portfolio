using Application.Common.Entities;
using Application.Profile.Commands;
using Application.Profile.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Profile.Handlers
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
            var Output = new AbstractViewModel();
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
                    Output.lstError.Add("Education not found");
                    return Output;
                }

                _mapper.Map(request, oldEducation);
            }

            try
            {
                await _context.SaveChangesAsync();
                Output.status = true;
            }
            catch
            {
                Output.status = false;
                Output.lstError.Add("Error while adding/updating the Education");
            }

            return Output;
        }
    }
}
