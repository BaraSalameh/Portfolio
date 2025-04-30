using Application.Client.Queries;
using Application.Common.Entities;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class UserByUsernameQueryHandler : IRequestHandler<UserByUsernameQuery, SingleQueryResponse<UBUQ_Response>>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UserByUsernameQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<SingleQueryResponse<UBUQ_Response>> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var response = new SingleQueryResponse<UBUQ_Response>();

            try
            {
                var existingEntity = await _context.User
                    .Where(u => u.Username == request.Username)
                    .Include(u => u.LstProjects).ThenInclude(p => p.LstProjectTechnologies).ThenInclude(pt => pt.LKP_Technology)
                    .Include(u => u.LstSkills)
                    .Include(u => u.LstEducations)
                    .Include(u => u.LstExperiences)
                    .Include(u => u.LstBlogPosts)
                    .Include(u => u.LstSocialLinks)
                    .Include(u => u.LstUserLanguages).ThenInclude(ul => ul.LKP_Language)
                    .Include(u => u.LstUserLanguages).ThenInclude(ul => ul.LKP_LanguageProficiency)
                    .FirstOrDefaultAsync(cancellationToken);

                if(existingEntity == null)
                {
                    response.lstError.Add("Wrong username.");
                    return response;
                }

                response.Data = _mapper.Map<UBUQ_Response>(existingEntity);
            }
            catch
            {
                response.lstError.Add("Unexpected error occurred.");
            }
            
            return response;
        }
    }
}
