

using MediatR;
using System;

namespace TarotNow.Application.Features.Admin.Commands.ToggleUserLock;

public class ToggleUserLockCommand : IRequest<bool>
{
        public Guid UserId { get; set; }

        public bool Lock { get; set; }
}
