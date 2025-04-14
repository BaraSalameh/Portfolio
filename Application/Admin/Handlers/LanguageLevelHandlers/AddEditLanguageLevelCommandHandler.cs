using Application.Admin.Commands.LanguageLevelCommands;
using Application.Admin.MappingProfiles;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LanguageLevelHandlers
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
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_LanguageLevel>(request);

            if (request.ID == null)
            {
                _context.LKP_LanguageLevel.Add(ResultToDB);
            }
            else
            {
                var oldLanguageLevel =
                    await _context.LKP_LanguageLevel
                        .Where(x => x.ID == request.ID && x.IsDeleted == false)
                        .FirstOrDefaultAsync();

                if (oldLanguageLevel == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("Language level not found");
                    return Vm;
                }

                _mapper.Map(request, oldLanguageLevel);
                oldLanguageLevel.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the language level");
            }

            return Vm;
        }
    }
}
