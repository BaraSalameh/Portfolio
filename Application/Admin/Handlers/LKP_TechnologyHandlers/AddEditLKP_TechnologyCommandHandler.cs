using Application.Admin.Commands.LKP_TechnologyCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_TechnologyHandlers
{
    public class AddEditLKP_TechnologyCommandHandler : IRequestHandler<AddEditLKP_TechnologyCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLKP_TechnologyCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<CommandResponse> Handle(AddEditLKP_TechnologyCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<LKP_Technology>(request);
                    await _context.LKP_Technology.AddAsync(newEntity, cancellationToken);
                }
                else
                {
                    var existingEntity =
                        await _context.LKP_Technology
                            .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                    if (existingEntity == null)
                    {
                        response.lstError.Add("LKP_Technology not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

            
                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the LKP_Technology.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
