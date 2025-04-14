using Application.Common.Entities;
using Application.CV.Commands.LanguageCommands;
using Application.CV.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CV.Handlers.LanguageHandlers
{
    public class AddEditLanguageCommandHandler : IRequestHandler<AddEditLanguageCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLanguageCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new LanguageMappingProfiles().AddEditLanguageCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(AddEditLanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<Language>(request);

            if (request.ID == null)
            {
                _context.Language.Add(ResultToDB);
            }
            else
            {
                var oldLanguage =
                    await _context.Language
                        .FindAsync(request.ID);

                if (oldLanguage == null)
                {
                    Vm.lstError.Add("Language not found");
                    Vm.status = false;
                    return Vm;
                }

                _mapper.Map(request, oldLanguage);
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Language");
            }

            return Vm;
        }
    }
}
