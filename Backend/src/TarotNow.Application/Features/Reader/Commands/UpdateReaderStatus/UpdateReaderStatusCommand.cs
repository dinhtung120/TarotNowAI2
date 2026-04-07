

using MediatR;
using System;

namespace TarotNow.Application.Features.Reader.Commands.UpdateReaderStatus;

public class UpdateReaderStatusCommand : IRequest<bool>
{
        public Guid UserId { get; set; }

        public string Status { get; set; } = string.Empty;
}
