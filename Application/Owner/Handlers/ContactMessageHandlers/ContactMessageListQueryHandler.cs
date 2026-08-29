using Application.Common.Services.Interface;
using Application.Owner.Queries.ContactMessageQueries;
using AutoMapper;
using Application.Common.Persistence;
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

            var counts = await existingEntity
                .GroupBy(_ => 1)
                .Select(group => new
                {
                    RowCount = group.Count(),
                    UnreadCount = group.Count(message => !message.IsRead)
                })
                .SingleOrDefaultAsync(cancellationToken);
            response.UnreadContactMessageCount = counts?.UnreadCount ?? 0;
            response.RowCount = counts?.RowCount ?? 0;
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<CMLQ_ContactMessage>(
                    existingEntity
                        .OrderByDescending(e => e.CreatedAt)
                        .ThenBy(e => e.ID)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
