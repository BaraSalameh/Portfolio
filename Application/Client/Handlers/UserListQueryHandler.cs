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
                var StrSearch = request.Search;
                Filter = f =>
                    (f.Email ?? "").Contains(StrSearch) ||
                    (f.Firstname ?? "").Contains(StrSearch) ||
                    (f.Lastname ?? "").Contains(StrSearch);
            }

            var wxistingEntity =_context.User
                .Where(Filter);

            response.Items =
                await _mapper.ProjectTo<ULQ_Response>(
                    wxistingEntity.Skip((int)(request.PageNumber * request.PageSize)!).Take((int)request.PageSize!)
                ).ToListAsync();
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
