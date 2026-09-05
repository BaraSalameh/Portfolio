using Application.Common.Entities;
using Application.Common.Services.Interface;
using Application.Owner.Commands.Profile;
using AutoMapper;
using Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.Profile
{
    public class EditProfileCommandHandler : IRequestHandler<EditProfileCommand, CommandResponse>
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICloudinaryAssetService _cloudinaryAssets;

        public EditProfileCommandHandler(
            IAppDbContext context,
            ICurrentUserService currentUserService,
            IMapper mapper,
            ICloudinaryAssetService cloudinaryAssets)
        {
            _currentUserService = currentUserService;
            _context = context;
            _mapper = mapper;
            _cloudinaryAssets = cloudinaryAssets;
        }

        public async Task<CommandResponse> Handle(EditProfileCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();

            var existingEntity = await _context.User
                .FirstOrDefaultAsync(u => u.ID == _currentUserService.UserID!.Value, cancellationToken);

            if (existingEntity == null)
            {
                response.lstError.Add("User not found.");
                return response;
            }

            var previousProfilePicture = existingEntity.ProfilePicture;
            var previousCoverPhoto = existingEntity.CoverPhoto;

            _mapper.Map(request, existingEntity);

            await _context.SaveChangesAsync(cancellationToken);

            var cleanupTasks = new List<Task>(2);
            if (!string.Equals(previousProfilePicture, existingEntity.ProfilePicture, StringComparison.Ordinal))
            {
                cleanupTasks.Add(_cloudinaryAssets.DeleteByUrlAsync(previousProfilePicture, CancellationToken.None));
            }
            if (!string.Equals(previousCoverPhoto, existingEntity.CoverPhoto, StringComparison.Ordinal))
            {
                cleanupTasks.Add(_cloudinaryAssets.DeleteByUrlAsync(previousCoverPhoto, CancellationToken.None));
            }
            await Task.WhenAll(cleanupTasks);

            return response;
        }
    }
}
