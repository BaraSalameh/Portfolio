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
            Expression<Func<User, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var search = request.Search.ToLower();
                Filter = f =>
                    f.Username.ToLower().Contains(search) ||
                    f.Email.ToLower().Contains(search) ||
                    f.Firstname.ToLower().Contains(search) ||
                    f.Lastname.ToLower().Contains(search);
            }

            var existingEntity =_context.User.Where(Filter);

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
