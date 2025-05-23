﻿using Application.Common.Entities;
using MediatR;

namespace Application.Admin.Commands.LKP_TechnologyCommands
{
    public class AddEditLKP_TechnologyCommand : IRequest<CommandResponse>
    {
        public Guid? ID { get; set; }
        public string Name { get; set; }
        public string IconUrl { get; set; }
    }
}
