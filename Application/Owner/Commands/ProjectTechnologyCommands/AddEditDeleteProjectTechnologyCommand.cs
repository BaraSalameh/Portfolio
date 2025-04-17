﻿using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.ProjectTechnologyCommands
{
    public class AddEditDeleteProjectTechnologyCommand : IRequest<AbstractViewModel>
    {
        public Guid? ID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string LiveLink { get; set; }
        public string SourceCode { get; set; }
        public string ImageUrl { get; set; }
        public bool IsFeatured { get; set; }
        public List<Guid>? LstProjectTechnologies { get; set; }
    }
}
