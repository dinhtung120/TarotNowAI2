

using MediatR;
using System;

namespace TarotNow.Application.Features.Reading.Commands.RevealSession;

public class RevealReadingSessionCommand : IRequest<RevealReadingSessionResult>
{
        public Guid UserId { get; set; }

        public string SessionId { get; set; } = string.Empty;
}

public class RevealReadingSessionResult
{
        public int[] Cards { get; set; } = Array.Empty<int>();
}
