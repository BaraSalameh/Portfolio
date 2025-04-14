using Application.Common.Entities;
using MediatR;

namespace Application.CV.Commands.HeaderCommands
{
    public class AddEditHeaderCommand : IRequest<AbstractViewModel>
    {
        public int? ID { get; set; }
        public string? ImagePath { get; set; }
        public string? Title { get; set; }
        public int UserID { get; set; }
    }
}
