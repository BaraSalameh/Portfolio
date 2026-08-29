using Application.Client.Queries;
using Application.Common.Entities;
using AutoMapper;
using Application.Common.Persistence;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Client.Handlers
{
    public class UserListQueryHandler : IRequestHandler<UserListQuery, ListQueryResponse<ULQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UserListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<ULQ_Response>> Handle(UserListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<ULQ_Response>();

            var existingEntity = _context.User
                .AsNoTracking()
                .Where(u => u.IsConfirmed);

            if (!string.IsNullOrEmpty(request.Search))
            {
                existingEntity = PublicUserSearch.Apply(existingEntity, request.Search);
            }

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<ULQ_Response>(
                    existingEntity
                        .OrderBy(u => u.CreatedAt)
                        .ThenBy(u => u.ID)
                        .Skip(request.Offset)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
