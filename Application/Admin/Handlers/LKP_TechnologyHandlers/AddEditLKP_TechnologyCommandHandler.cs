using Application.Admin.Commands.LKP_TechnologyCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_TechnologyHandlers
{
    public class AddEditLKP_TechnologyCommandHandler : IRequestHandler<AddEditLKP_TechnologyCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        public AddEditLKP_TechnologyCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<AbstractViewModel> Handle(AddEditLKP_TechnologyCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<LKP_Technology>(request);

            if (request.ID == null)
            {
                await _context.LKP_Technology.AddAsync(ResultToDB);
            }
            else
            {
                var oldLKP_Technology =
                    await _context.LKP_Technology
                        .Where(x => x.ID == request.ID && (x.IsDeleted == false || x.IsDeleted == null))
                        .FirstOrDefaultAsync();

                if (oldLKP_Technology == null)
                {
                    Vm.status = false;
                    Vm.lstError.Add("LKP_Technology not found");
                    return Vm;
                }

                _mapper.Map(request, oldLKP_Technology);
                oldLKP_Technology.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the LKP_Technology");
            }

            return Vm;
        }
    }
}
