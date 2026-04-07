using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.RespondCall;

public class RespondCallCommand : IRequest<CallSessionDto>
{
        public string CallSessionId { get; set; } = string.Empty;

        public Guid ResponderId { get; set; }

        public bool Accept { get; set; }
}
