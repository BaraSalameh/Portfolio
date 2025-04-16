using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageProficiencyHandlers
{
    public class AddEditLKP_LanguageProficiencyCommandHandler : IRequestHandler<AddEditLKP_LanguageProficiencyCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public AddEditLKP_LanguageProficiencyCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<AbstractViewModel> Handle(AddEditLKP_LanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_LanguageProficiency>(request);

            if (request.ID == null)
            {
                await _context.LKP_LanguageProficiency.AddAsync(ResultToDB);
            }
            else
            {
                var oldLKP_LanguageProficiency =
                    await _context.LKP_LanguageProficiency
                        .Where(x => x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                        .FirstOrDefaultAsync();

                if (oldLKP_LanguageProficiency == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("LKP_LanguageProficiency not found");
                    return Vm;
                }

                _mapper.Map(request, oldLKP_LanguageProficiency);
                oldLKP_LanguageProficiency.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the LKP_LanguageProficiency");
            }

            return Vm;
        }
    }
}
