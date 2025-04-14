using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.LinkCommands
{
    public class AddEditLinkCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? Path { get; set; }
        public int ProfileID { get; set; }
    }
}
