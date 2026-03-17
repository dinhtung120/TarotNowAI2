using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.ConfirmRelease;

/// <summary>
/// Command: User confirm → release diamond cho reader (- 10% platform fee).
///
/// Business rules:
/// → Chỉ user (payer) mới confirm được.
/// → Item phải ở status = accepted VÀ reader đã reply.
/// → Release qua proc_wallet_release (ACID).
/// → Platform fee 10% — deducted từ số tiền reader nhận.
/// → Set dispute_window 24h sau release.
/// </summary>
public class ConfirmReleaseCommand : IRequest<bool>
{
    public Guid ItemId { get; set; }
    public Guid UserId { get; set; }
}

public class ConfirmReleaseCommandHandler : IRequestHandler<ConfirmReleaseCommand, bool>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;

    public ConfirmReleaseCommandHandler(IChatFinanceRepository financeRepo, IWalletRepository walletRepo)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
    }

    public async Task<bool> Handle(ConfirmReleaseCommand req, CancellationToken ct)
    {
        var item = await _financeRepo.GetItemByIdAsync(req.ItemId, ct)
            ?? throw new NotFoundException("Không tìm thấy câu hỏi.");

        if (item.PayerId != req.UserId)
            throw new BadRequestException("Chỉ người đặt câu hỏi mới được confirm release.");

        if (item.Status != QuestionItemStatus.Accepted)
            throw new BadRequestException($"Câu hỏi ở trạng thái {item.Status}, không thể release.");

        if (item.RepliedAt == null)
            throw new BadRequestException("Reader chưa trả lời. Không thể release.");

        if (item.ReleasedAt != null)
            throw new BadRequestException("Đã release rồi.");

        // Platform fee 10%
        var fee = (long)Math.Ceiling(item.AmountDiamond * 0.10);
        var readerAmount = item.AmountDiamond - fee;

        // Release: trừ frozen của payer, cộng vào reader (- fee)
        await _walletRepo.ReleaseAsync(
            item.PayerId, item.ReceiverId, readerAmount,
            referenceSource: "chat_question_item",
            referenceId: item.Id.ToString(),
            description: $"Release {readerAmount}💎 (fee {fee}💎) cho reader",
            idempotencyKey: $"release_{item.Id}",
            cancellationToken: ct);

        // Fee: consume phần fee (trừ khỏi frozen, không ai nhận)
        if (fee > 0)
        {
            await _walletRepo.ConsumeAsync(
                item.PayerId, fee,
                referenceSource: "platform_fee",
                referenceId: item.Id.ToString(),
                description: $"Platform fee 10% = {fee}💎",
                idempotencyKey: $"fee_{item.Id}",
                cancellationToken: ct);
        }

        var now = DateTime.UtcNow;
        item.Status = QuestionItemStatus.Released;
        item.ReleasedAt = now;
        item.ConfirmedAt = now;
        item.AutoReleaseAt = null;
        item.DisputeWindowStart = now;
        item.DisputeWindowEnd = now.AddHours(24);

        await _financeRepo.UpdateItemAsync(item, ct);

        // Cập nhật session total_frozen
        var session = await _financeRepo.GetSessionByIdAsync(item.FinanceSessionId, ct);
        if (session != null)
        {
            session.TotalFrozen -= item.AmountDiamond;
            if (session.TotalFrozen < 0) session.TotalFrozen = 0;
            await _financeRepo.UpdateSessionAsync(session, ct);
        }

        await _financeRepo.SaveChangesAsync(ct);
        return true;
    }
}
