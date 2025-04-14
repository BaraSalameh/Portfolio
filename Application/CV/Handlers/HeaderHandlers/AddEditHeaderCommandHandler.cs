using Application.Common.Entities;
using Application.CV.Commands.HeaderCommands;
using Application.CV.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;

namespace Application.CV.Handlers.HeaderHandlers
{
    public class AddEditHeaderCommandHandler : IRequestHandler<AddEditHeaderCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditHeaderCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new HeaderMappingProfiles().AddEditHeaderCommandHandler();

        }

        public async Task<AbstractViewModel> Handle(AddEditHeaderCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<Domain.Entities.Profile>(request);

            if (request.ID == null)
            {
                _context.Profile.Add(ResultToDB);
            }
            else
            {
                var oldHeader =
                    await _context.Profile
                        .FindAsync(request.ID);

                if (oldHeader == null)
                {
                    Vm.lstError.Add("Header not found");
                    Vm.status = false;
                    return Vm;
                }

                _mapper.Map(request, oldHeader);
                oldHeader.UpdatedAt = DateTime.UtcNow;
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Header");
            }

            return Vm;
        }
    }
}
