using Application.Common.Entities;
using Application.Owner.Queries.EducationQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.EducationHandlers
{
    public class EducationListQueryHandler : IRequestHandler<EducationListQuery, ListQueryResponse<ELQ_Educations>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public EducationListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<ELQ_Educations>> Handle(EducationListQuery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<ELQ_Educations>();

            var existingEntity = _context.User
                .AsNoTracking()
                .Where(u => u.Username == request.Username)
                .SelectMany(u => u.LstEducations)
                .Where(ed => ed.IsDeleted == false);

            response.Items = await _mapper.ProjectTo<ELQ_Educations>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
