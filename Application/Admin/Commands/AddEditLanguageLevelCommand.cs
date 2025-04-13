using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands
{
    public class AddEditLanguageLevelCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? Level { get; set; }
    }
}
