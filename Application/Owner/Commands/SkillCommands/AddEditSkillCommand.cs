using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SkillCommands
{
    public class AddEditSkillCommand : IRequest<AbstractViewModel>
    {
        public Guid? ID { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Proficiency { get; set; }
        public string IconUrl { get; set; }
    }
}
