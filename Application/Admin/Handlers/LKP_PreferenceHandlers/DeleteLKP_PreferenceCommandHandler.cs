﻿using Application.Admin.Commands.LKP_PreferenceCommands;
using Application.Common.Entities;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Admin.Handlers.LKP_PreferenceHandlers
{
    public class DeleteLKP_PreferenceCommandHandler : IRequestHandler<DeleteLKP_PreferenceCommand, CommandResponse>
    {
        private readonly IAppDbContext _context;

        public DeleteLKP_PreferenceCommandHandler(IAppDbContext context)
        {
            _context = context;
        }

        public async Task<CommandResponse> Handle(DeleteLKP_PreferenceCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            try
            {
                var existingEntity = await _context.LKP_Preference
                    .FirstOrDefaultAsync(x => x.ID == request.ID && x.IsDeleted == false, cancellationToken);

                if (existingEntity == null)
                {
                    response.lstError.Add("LKP_Preference not found.");
                    return response;
                }

                existingEntity.IsDeleted = true;
                existingEntity.DeletedAt = DateTime.UtcNow;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException dbEx)
            {
                response.lstError.Add("Error while deleting the LKP_Preference.");
            }
            catch (Exception ex)
            {
                response.lstError.Add("Unexpected error occurred.");
            }

            return response;
        }
    }
}
