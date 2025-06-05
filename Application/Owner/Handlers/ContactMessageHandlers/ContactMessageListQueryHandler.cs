using Application.Common.Services.Interface;
using Application.Owner.Queries.ContactMessageQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.ContactMessageHandlers
{
    public class ContactMessageListQueryHandler : IRequestHandler<ContactMessageListQuery, CMLQ_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;

        public ContactMessageListQueryHandler(IAppDbContext context, IMapper mapper, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _currentUserService = currentUserService;
        }

        public async Task<CMLQ_Response> Handle(ContactMessageListQuery request, CancellationToken cancellationToken)
        {
            var response = new CMLQ_Response();

            var existingEntity = _context.ContactMessage
                .AsNoTracking()
                .Where(e => e.UserID == _currentUserService.UserID && e.IsDeleted == false);

            response.UnreadContactMessageCount = await existingEntity.CountAsync(cm => !cm.IsRead, cancellationToken);
            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<CMLQ_ContactMessage>(
                    existingEntity
                        .OrderByDescending(e => e.CreatedAt)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
