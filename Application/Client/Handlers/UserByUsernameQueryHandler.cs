using Application.Client.Queries;
using AutoMapper;
using DataAccess.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Client.Handlers
{
    public class UserByUsernameQueryHandler : IRequestHandler<UserByUsernameQuery, UBUQ_Response>
    {
        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;

        public UserByUsernameQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<UBUQ_Response> Handle(UserByUsernameQuery request, CancellationToken cancellationToken)
        {
            var Vm = new UBUQ_Response();

            var ResultFromDB = await _context.User
                .Where(u => u.Username == request.Username)
                .Include(u => u.LstProjects).ThenInclude(p => p.LstProjectTechnologies).ThenInclude(pt => pt.LKP_Technology)
                .Include(u => u.LstSkills)
                .Include(u => u.LstEducations)
                .Include(u => u.LstExperiences)
                .Include(u => u.LstBlogPosts)
                .Include(u => u.LstSocialLinks)
                .Include(u=> u.LstUserLanguages).ThenInclude(ul => ul.LKP_Language)
                .Include(u=> u.LstUserLanguages).ThenInclude(ul => ul.LKP_LanguageProficiency)
                .FirstOrDefaultAsync(cancellationToken);

            Vm = _mapper.Map<UBUQ_Response>(ResultFromDB);

            return Vm;
        }
    }
}
