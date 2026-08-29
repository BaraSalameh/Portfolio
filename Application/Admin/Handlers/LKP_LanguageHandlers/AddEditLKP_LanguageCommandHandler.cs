using Application.Admin.Commands.LKP_LanguageCommands;
using Application.Common.Entities;
using Application.Common.Functions;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageHandlers
{
    public class AddEditLKP_LanguageCommandHandler : IRequestHandler<AddEditLKP_LanguageCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLKP_LanguageCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditLKP_LanguageCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            request.Name = request.Name.ToPascalCase();

            if (request.ID == null)
            {
                var newEntity = _mapper.Map<LKP_Language>(request);
                await _context.LKP_Language.AddAsync(newEntity, cancellationToken);
            }
            else
            {
                var existingEntity = await _context.LKP_Language
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_Language not found.");
                    return response;
                }

                _mapper.Map(request, existingEntity);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return response;
        }
    }
}
