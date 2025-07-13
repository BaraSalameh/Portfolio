using Application.Common.Entities;
using Application.Owner.Queries.UserPreferenceQueries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Owner.Handlers.UserPreferenceHandlers
{
    public class LKP_PreferenceListQueryHandler : IRequestHandler<LKP_PreferenceListQurery, ListQueryResponse<LKP_PLQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public LKP_PreferenceListQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ListQueryResponse<LKP_PLQ_Response>> Handle(LKP_PreferenceListQurery request, CancellationToken cancellationToken)
        {
            var response = new ListQueryResponse<LKP_PLQ_Response>();

            var existingEntity = _context.LKP_Preference
                .AsNoTracking();

            response.Items = await _mapper.ProjectTo<LKP_PLQ_Response>(existingEntity).ToListAsync(cancellationToken);
            response.RowCount = response.Items.Count();

            return response;
        }
    }
}
