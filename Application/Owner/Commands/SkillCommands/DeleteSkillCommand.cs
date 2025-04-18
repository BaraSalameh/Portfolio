﻿using Application.Common.Entities;
using MediatR;

namespace Application.Owner.Commands.SkillCommands
{
    public class DeleteSkillCommand : IRequest<AbstractViewModel>
    {
        public Guid ID { get; set; }
    }
}
