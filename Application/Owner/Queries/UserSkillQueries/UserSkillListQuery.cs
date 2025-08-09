using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Queries.UserSkillQueries
{
    public class UserSkillListQuery : IRequest<ListQueryResponse<USLQ_Response>> { }

    public class USLQ_Response
    {
        public USLQ_LKP_Skill Skill { get; set; }
        public List<USLQ_Education> LstEducations { get; set; }
        public List<USLQ_Experience> LstExperiences { get; set; }
        public List<USLQ_Project> LstProjects { get; set; }
        public List<USLQ_Certificate> LstCertificates { get; set; }
    }

    public class USLQ_LKP_Skill
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }

    public class USLQ_Education
    {
        public Guid ID { get; set; }
        public USLQ_LKP_Institution Institution { get; set; }
    }

    public class USLQ_LKP_Institution
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public string? Logo { get; set; }
    }

    public class USLQ_Experience
    {
        public Guid ID { get; set; }
        public string CompanyName { get; set; }
    }

    public class USLQ_Project
    {
        public Guid ID { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
    }

    public class USLQ_Certificate
    {
        public Guid ID { get; set; }
        public USLQ_LKP_Certificate Certificate { get; set; }
    }

    public class USLQ_LKP_Certificate
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
    }
}
