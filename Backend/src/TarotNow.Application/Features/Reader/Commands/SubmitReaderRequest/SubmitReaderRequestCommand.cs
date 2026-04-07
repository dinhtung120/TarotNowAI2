

using MediatR;
using System;
using System.Collections.Generic;

namespace TarotNow.Application.Features.Reader.Commands.SubmitReaderRequest;

public class SubmitReaderRequestCommand : IRequest<bool>
{
        public Guid UserId { get; set; }

        public string IntroText { get; set; } = string.Empty;

        public List<string> ProofDocuments { get; set; } = new();
}
