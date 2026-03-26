using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IReadingSessionOrchestrator
{
    Task<(bool Success, string ErrorMessage)> StartPaidSessionAsync(
        StartPaidSessionRequest request,
        CancellationToken cancellationToken = default);
}

public sealed class StartPaidSessionRequest
{
    public Guid UserId { get; init; }
    public string SpreadType { get; init; } = string.Empty;
    public ReadingSession Session { get; init; } = null!;
    public long CostGold { get; init; }
    public long CostDiamond { get; init; }
}
