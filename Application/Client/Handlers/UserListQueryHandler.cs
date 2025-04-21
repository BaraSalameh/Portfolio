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
    public class UserListQueryHandler : IRequestHandler<ListQuery<ULQ_Response>, ListQuery_Response<ULQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UserListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQuery_Response<ULQ_Response>> Handle(ListQuery<ULQ_Response> request, CancellationToken cancellationToken)
        {
            var Vm = new ListQuery_Response<ULQ_Response>();
            Expression<Func<User, bool>> Filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var StrSearch = request.Search;
                Filter = f =>
                    (f.Email ?? "").Contains(StrSearch) ||
                    (f.Firstname ?? "").Contains(StrSearch) ||
                    (f.Lastname ?? "").Contains(StrSearch);
            }

            var ResultFromDB =
                _context.User
                    .Where(u => u.IsActive == true)
                    .Where(Filter);

            Vm.Items =
                await _mapper.ProjectTo<ULQ_Response>(
                    ResultFromDB.Skip((int)(request.PageNumber * request.PageSize)!).Take((int)request.PageSize!)
                ).ToListAsync();
            Vm.RowCount = Vm.Items.Count();

            return Vm;
        }
    }
}
