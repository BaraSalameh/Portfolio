using Application.Common.Entities;
using Application.CV.Commands.LinkCommands;
using Application.CV.MappingProfiles;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;

namespace Application.CV.Handlers.LinkHandlers
{
    public class AddEditLinkCommandHandler : IRequestHandler<AddEditLinkCommand, AbstractViewModel>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLinkCommandHandler(IAppDbContext context)
        {
            _context = context;
            _mapper = new LinkMappingProfiles().AddEditLinkCommandHandler();
        }

        public async Task<AbstractViewModel> Handle(AddEditLinkCommand request, CancellationToken cancellationToken)
        {
            var Vm = new AbstractViewModel();
            var ResultToDB = _mapper.Map<Link>(request);

            if (request.ID == null)
            {
                _context.Link.Add(ResultToDB);
            }
            else
            {
                var oldLink =
                    await _context.Link
                        .FindAsync(request.ID);

                if (oldLink == null)
                {
                    Vm.lstError.Add("Link not found");
                    Vm.status = false;
                    return Vm;
                }

                _mapper.Map(request, oldLink);
            }

            try
            {
                await _context.SaveChangesAsync();
                Vm.status = true;
            }
            catch
            {
                Vm.status = false;
                Vm.lstError.Add("Error while adding/updating the Link");
            }

            return Vm;
        }
    }
}
