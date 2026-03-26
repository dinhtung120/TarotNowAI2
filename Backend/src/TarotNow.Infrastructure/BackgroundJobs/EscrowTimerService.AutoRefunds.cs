using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Infrastructure.BackgroundJobs;

public partial class EscrowTimerService
{
    private readonly record struct AutoRefundExecutionContext(
        IChatFinanceRepository FinanceRepository,
        IWalletRepository WalletRepository,
        ITransactionCoordinator TransactionCoordinator,
        IDomainEventPublisher DomainEventPublisher,
        CancellationToken CancellationToken);

    private async Task ProcessAutoRefunds(
        IChatFinanceRepository repo,
        IWalletRepository wallet,
        ITransactionCoordinator transactionCoordinator,
        IDomainEventPublisher domainEventPublisher,
        CancellationToken cancellationToken)
    {
        var context = new AutoRefundExecutionContext(
            repo,
            wallet,
            transactionCoordinator,
            domainEventPublisher,
            cancellationToken);
        var candidates = await repo.GetItemsForAutoRefundAsync(cancellationToken);
        foreach (var candidate in candidates)
        {
            try
            {
                await ProcessAutoRefundCandidateAsync(context, candidate.Id);

                _logger.LogInformation(
                    "[EscrowTimer] Auto-refund: {ItemId}, {Amount}💎",
                    candidate.Id,
                    candidate.AmountDiamond);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "[EscrowTimer] Auto-refund failed: {ItemId}", candidate.Id);
            }
        }
    }

    private static async Task ProcessAutoRefundCandidateAsync(
        AutoRefundExecutionContext context,
        Guid candidateId)
    {
        await context.TransactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await context.FinanceRepository.GetItemForUpdateAsync(candidateId, transactionCt);
            if (item == null || !IsEligibleForAutoRefund(item, DateTime.UtcNow))
            {
                return;
            }

            await context.WalletRepository.RefundAsync(
                item.PayerId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Auto-refund {item.AmountDiamond}💎 (reader không reply trong 24h)",
                idempotencyKey: $"settle_refund_{item.Id}",
                cancellationToken: transactionCt);

            ApplyRefundState(item, DateTime.UtcNow);
            await context.FinanceRepository.UpdateItemAsync(item, transactionCt);
            await UpdateSessionFrozenBalanceAsync(context.FinanceRepository, item, transactionCt);
            await context.FinanceRepository.SaveChangesAsync(transactionCt);
            await PublishAutoRefundEventAsync(context.DomainEventPublisher, item, transactionCt);
        }, context.CancellationToken);
    }

    private static bool IsEligibleForAutoRefund(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        return item.Status == QuestionItemStatus.Accepted
               && item.RepliedAt == null
               && item.AutoRefundAt != null
               && item.AutoRefundAt <= now;
    }

    private static void ApplyRefundState(Domain.Entities.ChatQuestionItem item, DateTime now)
    {
        item.Status = QuestionItemStatus.Refunded;
        item.RefundedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
    }

    private static Task PublishAutoRefundEventAsync(
        IDomainEventPublisher domainEventPublisher,
        Domain.Entities.ChatQuestionItem item,
        CancellationToken cancellationToken)
    {
        return domainEventPublisher.PublishAsync(new EscrowRefundedDomainEvent
        {
            ItemId = item.Id,
            UserId = item.PayerId,
            AmountDiamond = item.AmountDiamond,
            RefundSource = "auto_refund_timeout"
        }, cancellationToken);
    }
}
