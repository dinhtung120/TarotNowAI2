using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.InitiateCall;

public class InitiateCallCommand : IRequest<CallSessionDto>
{
        public string ConversationId { get; set; } = string.Empty;

        public Guid InitiatorId { get; set; }

        public string Type { get; set; } = "audio";
}
