using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Services;

public sealed class EscrowSettlementService : IEscrowSettlementService
{
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IWalletRepository _walletRepository;

    public EscrowSettlementService(
        IChatFinanceRepository financeRepository,
        IWalletRepository walletRepository)
    {
        _financeRepository = financeRepository;
        _walletRepository = walletRepository;
    }

    public async Task ApplyReleaseAsync(
        ChatQuestionItem item,
        bool isAutoRelease,
        CancellationToken cancellationToken = default)
    {
        var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
        var readerAmount = item.AmountDiamond - fee;
        var (releaseDescription, feeDescription) = BuildDescriptions(isAutoRelease, readerAmount, fee);

        await _walletRepository.ReleaseAsync(
            item.PayerId,
            item.ReceiverId,
            readerAmount,
            referenceSource: "chat_question_item",
            referenceId: item.Id.ToString(),
            description: releaseDescription,
            idempotencyKey: $"settle_release_{item.Id}",
            cancellationToken: cancellationToken);

        if (fee > 0)
        {
            await _walletRepository.ConsumeAsync(
                item.PayerId,
                fee,
                referenceSource: "platform_fee",
                referenceId: item.Id.ToString(),
                description: feeDescription,
                idempotencyKey: $"settle_fee_{item.Id}",
                cancellationToken: cancellationToken);
        }

        ApplyReleasedState(item, isAutoRelease);

        await _financeRepository.UpdateItemAsync(item, cancellationToken);
        await DecreaseSessionFrozenAsync(item, cancellationToken);
    }

    private static (string ReleaseDescription, string FeeDescription) BuildDescriptions(
        bool isAutoRelease,
        long readerAmount,
        long fee)
    {
        if (isAutoRelease)
        {
            return ($"Auto-release {readerAmount}💎 (fee {fee}💎)", $"Platform fee auto 10% = {fee}💎");
        }

        return ($"Release {readerAmount}💎 (fee {fee}💎) cho reader", $"Platform fee 10% = {fee}💎");
    }

    private static void ApplyReleasedState(ChatQuestionItem item, bool isAutoRelease)
    {
        var now = DateTime.UtcNow;
        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = now;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);
        item.AutoReleaseAt = null;

        if (!isAutoRelease)
        {
            item.ConfirmedAt = now;
        }
    }

    private async Task DecreaseSessionFrozenAsync(ChatQuestionItem item, CancellationToken cancellationToken)
    {
        var session = await _financeRepository.GetSessionForUpdateAsync(item.FinanceSessionId, cancellationToken);
        if (session == null)
        {
            return;
        }

        session.TotalFrozen -= item.AmountDiamond;
        if (session.TotalFrozen < 0)
        {
            session.TotalFrozen = 0;
        }

        await _financeRepository.UpdateSessionAsync(session, cancellationToken);
    }
}
