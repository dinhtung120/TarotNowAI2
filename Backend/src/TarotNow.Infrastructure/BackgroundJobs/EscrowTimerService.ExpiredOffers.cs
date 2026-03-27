using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private async Task ProcessExpiredOffers(
        RefundDependencies dependencies,
        CancellationToken cancellationToken)
    {
        var expired = await dependencies.FinanceRepository.GetExpiredOffersAsync(cancellationToken);
        foreach (var item in expired)
        {
            try
            {
                await ProcessExpiredOfferCandidateAsync(
                    dependencies,
                    item.Id,
                    cancellationToken);

                _logger.LogInformation("[EscrowTimer] Expired offer cancelled: {ItemId}", item.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Failed to cancel offer: {ItemId}", item.Id);
            }
        }
    }

    private async Task ProcessExpiredOfferCandidateAsync(
        RefundDependencies dependencies,
        Guid candidateId,
        CancellationToken cancellationToken)
    {
        var outcome = await ExecuteExpiredOfferRefundAsync(dependencies, candidateId, cancellationToken);
        if (string.IsNullOrWhiteSpace(outcome.ConversationId) == false && outcome.RefundedAmount > 0)
        {
            await MarkConversationExpiredAsync(
                dependencies,
                outcome.ConversationId,
                $"Reader không phản hồi trong thời gian quy định. Đã hoàn {outcome.RefundedAmount} 💎.",
                cancellationToken);
        }
    }
}
