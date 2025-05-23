﻿using Application.Common.Entities;
using MediatR;

namespace Application.Account.Commands
{
    public class LogoutCommand : IRequest<CommandResponse>{}
}
