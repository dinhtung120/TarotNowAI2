using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Services;

// Dịch vụ chốt settlement escrow để điều phối release tiền, fee và cập nhật trạng thái item.
public sealed partial class EscrowSettlementService : IEscrowSettlementService
{
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IWalletRepository _walletRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;
    private readonly ISystemConfigSettings _systemConfigSettings;

    /// <summary>
    /// Khởi tạo dịch vụ settlement escrow với các dependency tài chính và phát sự kiện domain.
    /// Luồng xử lý: nhận repository/publisher qua DI để service có thể cập nhật dữ liệu và publish event.
    /// </summary>
    public EscrowSettlementService(
        IChatFinanceRepository financeRepository,
        IWalletRepository walletRepository,
        IDomainEventPublisher domainEventPublisher,
        ISystemConfigSettings systemConfigSettings)
    {
        _financeRepository = financeRepository;
        _walletRepository = walletRepository;
        _domainEventPublisher = domainEventPublisher;
        _systemConfigSettings = systemConfigSettings;
    }

    /// <summary>
    /// Áp dụng release escrow cho một question item khi đủ điều kiện giải ngân.
    /// Luồng xử lý: tính fee theo rule, release tiền cho reader, trừ platform fee, cập nhật state và publish event.
    /// </summary>
    public async Task ApplyReleaseAsync(
        ChatQuestionItem item,
        bool isAutoRelease,
        CancellationToken cancellationToken = default)
    {
        var feeRate = _systemConfigSettings.WithdrawalFeeRate;
        var fee = (long)Math.Ceiling(item.AmountDiamond * (double)feeRate);
        var readerAmount = item.AmountDiamond - fee;
        var (releaseDescription, feeDescription) = BuildDescriptions(isAutoRelease, readerAmount, fee, feeRate);

        await _walletRepository.ReleaseAsync(
            item.PayerId,
            item.ReceiverId,
            readerAmount,
            referenceSource: "chat_question_item",
            referenceId: item.Id.ToString(),
            description: releaseDescription,
            idempotencyKey: $"settle_release_{item.Id}",
            cancellationToken: cancellationToken);
        // Hoàn tất nhánh chính: chuyển phần tiền thực nhận sang reader bằng idempotency key cố định.

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
            // Chỉ trừ platform fee khi fee dương để tránh phát sinh bút toán 0 giá trị.
        }

        ApplyReleasedState(item, isAutoRelease, _systemConfigSettings.EscrowDisputeWindowHours);
        // Chốt state item sau settlement trước khi ghi xuống repository để đảm bảo dữ liệu nhất quán.

        await _financeRepository.UpdateItemAsync(item, cancellationToken);
        await DecreaseSessionFrozenAsync(item, cancellationToken);
        await PublishReleasedEventAsync(item, readerAmount, fee, isAutoRelease, cancellationToken);
        // Đồng bộ persist + event để các luồng hậu xử lý đọc được trạng thái release mới nhất.
    }

    /// <summary>
    /// Áp dụng release escrow theo toàn bộ accepted item trong một finance session.
    /// Luồng xử lý: tính tổng gross/fee/released một lần, ghi bút toán ví gộp, cập nhật item/session và publish session event.
    /// </summary>
    public async Task<EscrowSessionReleaseSummary?> ApplySessionReleaseAsync(
        ChatFinanceSession session,
        IReadOnlyCollection<ChatQuestionItem> items,
        bool isAutoRelease,
        CancellationToken cancellationToken = default)
    {
        var acceptedItems = items
            .Where(item => item.FinanceSessionId == session.Id && item.Status == QuestionItemStatus.Accepted)
            .ToList();
        if (acceptedItems.Count == 0)
        {
            return null;
        }

        var feeRate = _systemConfigSettings.WithdrawalFeeRate;
        var (totalGross, totalFee, releasedAmount) = CalculateSessionAmounts(acceptedItems, feeRate);
        var (releaseDescription, feeDescription) = BuildSessionDescriptions(isAutoRelease, releasedAmount, totalFee, feeRate);

        await ApplySessionWalletSettlementAsync(
            session,
            releasedAmount,
            totalFee,
            releaseDescription,
            feeDescription,
            cancellationToken);

        var now = DateTime.UtcNow;
        await UpdateAcceptedItemsAsReleasedAsync(acceptedItems, isAutoRelease, now, cancellationToken);
        await UpdateSessionAfterReleaseAsync(session, totalGross, now, cancellationToken);

        var summary = CreateSessionReleaseSummary(session, totalGross, totalFee, releasedAmount, acceptedItems.Count, isAutoRelease);
        await PublishSessionReleasedEventAsync(summary, cancellationToken);
        return summary;
    }

    private static (long TotalGross, long TotalFee, long ReleasedAmount) CalculateSessionAmounts(
        IReadOnlyCollection<ChatQuestionItem> acceptedItems,
        decimal feeRate)
    {
        var totalGross = acceptedItems.Sum(item => item.AmountDiamond);
        var totalFee = (long)Math.Ceiling(totalGross * feeRate);
        var releasedAmount = Math.Max(0, totalGross - totalFee);
        return (totalGross, totalFee, releasedAmount);
    }

    private async Task ApplySessionWalletSettlementAsync(
        ChatFinanceSession session,
        long releasedAmount,
        long totalFee,
        string releaseDescription,
        string feeDescription,
        CancellationToken cancellationToken)
    {
        await _walletRepository.ReleaseAsync(
            session.UserId,
            session.ReaderId,
            releasedAmount,
            referenceSource: "chat_finance_session",
            referenceId: session.Id.ToString(),
            description: releaseDescription,
            idempotencyKey: $"settle_session_release_{session.Id}",
            cancellationToken: cancellationToken);

        if (totalFee <= 0)
        {
            return;
        }

        await _walletRepository.ConsumeAsync(
            session.UserId,
            totalFee,
            referenceSource: "platform_fee",
            referenceId: session.Id.ToString(),
            description: feeDescription,
            idempotencyKey: $"settle_session_fee_{session.Id}",
            cancellationToken: cancellationToken);
    }

    private async Task UpdateAcceptedItemsAsReleasedAsync(
        IReadOnlyCollection<ChatQuestionItem> acceptedItems,
        bool isAutoRelease,
        DateTime settledAtUtc,
        CancellationToken cancellationToken)
    {
        foreach (var acceptedItem in acceptedItems)
        {
            ApplyReleasedState(acceptedItem, isAutoRelease, _systemConfigSettings.EscrowDisputeWindowHours, settledAtUtc);
            await _financeRepository.UpdateItemAsync(acceptedItem, cancellationToken);
        }
    }

    private async Task UpdateSessionAfterReleaseAsync(
        ChatFinanceSession session,
        long totalGross,
        DateTime settledAtUtc,
        CancellationToken cancellationToken)
    {
        session.TotalFrozen = Math.Max(0, session.TotalFrozen - totalGross);
        if (session.TotalFrozen <= 0)
        {
            session.Status = ChatFinanceSessionStatus.Completed;
        }

        session.UpdatedAt = settledAtUtc;
        await _financeRepository.UpdateSessionAsync(session, cancellationToken);
    }

    private static EscrowSessionReleaseSummary CreateSessionReleaseSummary(
        ChatFinanceSession session,
        long totalGross,
        long totalFee,
        long releasedAmount,
        int releasedItemCount,
        bool isAutoRelease)
    {
        return new EscrowSessionReleaseSummary
        {
            FinanceSessionId = session.Id,
            PayerId = session.UserId,
            ReceiverId = session.ReaderId,
            GrossAmountDiamond = totalGross,
            FeeAmountDiamond = totalFee,
            ReleasedAmountDiamond = releasedAmount,
            ReleasedItemCount = releasedItemCount,
            IsAutoRelease = isAutoRelease
        };
    }
}
