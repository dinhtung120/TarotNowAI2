

using MediatR;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public class InitReadingSessionCommand : IRequest<InitReadingSessionResult>
{
    public Guid UserId { get; set; }
    
        public string SpreadType { get; set; } = string.Empty;
    
        public string? Question { get; set; }

        public string Currency { get; set; } = string.Empty;
}

public class InitReadingSessionResult
{
        public string SessionId { get; set; } = string.Empty;
    public long CostGold { get; set; }
    public long CostDiamond { get; set; }
}
