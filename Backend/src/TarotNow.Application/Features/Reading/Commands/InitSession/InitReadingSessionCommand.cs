using MediatR;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Reading.Commands.InitSession;

public class InitReadingSessionCommand : IRequest<InitReadingSessionResult>
{
    public Guid UserId { get; set; }
    public string SpreadType { get; set; }
    public string? Question { get; set; }
}

public class InitReadingSessionResult
{
    public string SessionId { get; set; } = string.Empty;
    public long CostGold { get; set; }
    public long CostDiamond { get; set; }
}
