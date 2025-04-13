using Application.Admin.Commands;
using Application.Admin.MappingProfiles;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.Admin.Handlers
{
    class AddEditLanguageLevelCommandHandler : IRequestHandler<AddEditLanguageLevelCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLanguageLevelCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new LanguageLevelMappingProfiles().AddEditLanguageLevelCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(AddEditLanguageLevelCommand request, CancellationToken cancellationToken)
        {
            var Output = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_LanguageLevel>(request);

            if (request.ID == null)
            {
                _context.LKP_LanguageLevel.Add(ResultToDB);
            }
            else
            {
                var oldLanguageLevel =
                    await _context.LKP_LanguageLevel
                        .FindAsync(request.ID);

                if (oldLanguageLevel == null)
                {
                    Output.lstError.Add("Language level not found");
                    return Output;
                }

                _mapper.Map(request, oldLanguageLevel);
            }

            try
            {
                await _context.SaveChangesAsync();
                Output.status = true;
            }
            catch
            {
                Output.status = false;
                Output.lstError.Add("Error while adding/updating the language level");
            }

            return Output;
        }
    }
}
