using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class EducationListQueryHandler : IRequestHandler<EducationListQuery, ListQuery_Response<ELQ_Educations>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EducationListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQuery_Response<ELQ_Educations>> Handle(EducationListQuery request, CancellationToken cancellationToken)
        {
            var Vm = new ListQuery_Response<ELQ_Educations>();

            Expression<Func<Education, bool>> filter = f => true;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var StrSearch = request.Search;
                filter = f =>
                    (f.Institution ?? "").Contains(StrSearch);
            }

            var ResultFromDB = _context.User
                .AsNoTracking()
                .Where(u => u.Username == request.Username)
                .SelectMany(u => u.LstEducations)
                .Where(filter);

            Vm.Items =
                await _mapper.ProjectTo<ELQ_Educations>(
                    ResultFromDB.Skip((int)(request.PageNumber * request.PageSize)!).Take((int)request.PageSize!)
                ).ToListAsync();
            Vm.RowCount = Vm.Items.Count();

            return Vm;
        }
    }
}
