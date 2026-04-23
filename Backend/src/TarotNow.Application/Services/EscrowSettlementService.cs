using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;

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
}
