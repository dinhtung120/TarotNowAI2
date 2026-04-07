using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Call.Commands.EndCall;

public class EndCallCommand : IRequest<CallSessionDto>
{
        public string CallSessionId { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public string Reason { get; set; } = "normal";
}
