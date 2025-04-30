using Application.Admin.Commands.LKP_LanguageProficiencyCommands;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_LanguageProficiencyHandlers
{
    public class AddEditLKP_LanguageProficiencyCommandHandler : IRequestHandler<AddEditLKP_LanguageProficiencyCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public AddEditLKP_LanguageProficiencyCommandHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            
        }
        public async Task<CommandResponse> Handle(AddEditLKP_LanguageProficiencyCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            
            try
            {
                if (request.ID == null)
                {
                    var newEntity = _mapper.Map<LKP_LanguageProficiency>(request);
                    await _context.LKP_LanguageProficiency.AddAsync(newEntity);
                }
                else
                {
                    var existingEntity = await _context.LKP_LanguageProficiency
                            .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                    if (existingEntity == null)
                    {
                        response.lstError.Add("LKP_LanguageProficiency not found.");
                        return response;
                    }

                    _mapper.Map(request, existingEntity);
                    existingEntity.UpdatedAt = DateTime.UtcNow;
                }

                await _context.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while adding/updating the LKP_LanguageProficiency.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
