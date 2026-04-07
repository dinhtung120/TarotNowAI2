using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

public interface IEscrowSettlementService
{
    Task ApplyReleaseAsync(
        ChatQuestionItem item,
        bool isAutoRelease,
        CancellationToken cancellationToken = default);
}

