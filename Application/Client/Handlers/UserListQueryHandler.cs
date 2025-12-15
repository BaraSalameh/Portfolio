using Application.Client.Queries;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
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

            var existingEntity = _context.User.Where(u => u.IsConfirmed);

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search;
                var terms = request.Search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                existingEntity = existingEntity.Where(u =>
                    u.Username.Contains(search) ||
                    u.Email.Contains(search) ||
                    terms.All(t => 
                        u.Firstname.Contains(t) || 
                        u.Lastname.Contains(t)
                    )
                );
            }

            response.RowCount = await existingEntity.CountAsync(cancellationToken);
            var pageNumber = request.PageNumber;
            var pageSize = request.PageSize;

            response.Items =
                await _mapper.ProjectTo<ULQ_Response>(
                    existingEntity
                        .OrderBy(u => u.CreatedAt)
                        .Skip(pageNumber * pageSize)
                        .Take(pageSize)
                ).ToListAsync(cancellationToken);

            return response;
        }
    }
}
