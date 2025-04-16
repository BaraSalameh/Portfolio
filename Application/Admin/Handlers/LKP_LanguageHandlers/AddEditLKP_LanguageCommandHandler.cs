using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageHandlers
{
    public class AddEditLKP_LanguageCommandHandler : IRequestHandler<AddEditLKP_LanguageCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public AddEditLKP_LanguageCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<AbstractViewModel> Handle(AddEditLKP_LanguageCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_Language>(request);

            if (request.ID == null)
            {
                await _context.LKP_Language.AddAsync(ResultToDB);
            }
            else
            {
                var oldLKP_Language =
                    await _context.LKP_Language
                        .Where(x => x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                        .FirstOrDefaultAsync();

                if (oldLKP_Language == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("LKP_Language not found");
                    return Vm;
                }

                _mapper.Map(request, oldLKP_Language);
                oldLKP_Language.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the LKP_Language");
            }

            return Vm;
        }
    }
}
