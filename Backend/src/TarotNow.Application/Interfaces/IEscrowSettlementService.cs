using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Shared settlement use case for manual confirm and auto-release flows.
/// </summary>
public interface IEscrowSettlementService
{
    Task ApplyReleaseAsync(
        ChatQuestionItem item,
        bool isAutoRelease,
        CancellationToken cancellationToken = default);
}

