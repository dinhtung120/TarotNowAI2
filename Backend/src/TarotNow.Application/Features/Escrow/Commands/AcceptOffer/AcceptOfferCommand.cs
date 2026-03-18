using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AcceptOffer;

/// <summary>
/// Command: User accept offer → freeze diamond, tạo finance session + question item.
/// Idempotency: idempotencyKey ngăn double-freeze.
/// </summary>
public class AcceptOfferCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public Guid ReaderId { get; set; }
    public string ConversationRef { get; set; } = string.Empty;
    public long AmountDiamond { get; set; }
    public string? ProposalMessageRef { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
}

public class AcceptOfferCommandHandler : IRequestHandler<AcceptOfferCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;

    public AcceptOfferCommandHandler(IChatFinanceRepository financeRepo, IWalletRepository walletRepo)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
    }

    public async Task<Guid> Handle(AcceptOfferCommand req, CancellationToken ct)
    {
        var idempotencyKey = req.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc.");

        if (idempotencyKey.Length > 128)
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");

        // 1. Idempotency — kiểm tra double-freeze
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null) return existing.Id;

        // 2. Lấy hoặc tạo finance session
        var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, ct);
        if (session == null)
        {
            session = new ChatFinanceSession
            {
                ConversationRef = req.ConversationRef,
                UserId = req.UserId,
                ReaderId = req.ReaderId,
                Status = "active",
                TotalFrozen = 0,
            };
            await _financeRepo.AddSessionAsync(session, ct);
        }

        // 3. Freeze diamond qua stored procedure
        await _walletRepo.FreezeAsync(
            req.UserId, req.AmountDiamond,
            referenceSource: "chat_question_item",
            referenceId: idempotencyKey,
            description: $"Escrow freeze {req.AmountDiamond}💎 cho conversation {req.ConversationRef}",
            idempotencyKey: $"freeze_{idempotencyKey}",
            cancellationToken: ct);

        // 4. Tạo question item
        var now = DateTime.UtcNow;
        var item = new ChatQuestionItem
        {
            FinanceSessionId = session.Id,
            ConversationRef = req.ConversationRef,
            PayerId = req.UserId,
            ReceiverId = req.ReaderId,
            Type = QuestionItemType.MainQuestion,
            AmountDiamond = req.AmountDiamond,
            Status = QuestionItemStatus.Accepted,
            ProposalMessageRef = req.ProposalMessageRef,
            AcceptedAt = now,
            ReaderResponseDueAt = now.AddHours(24),
            AutoRefundAt = now.AddHours(24),
            IdempotencyKey = idempotencyKey,
        };
        await _financeRepo.AddItemAsync(item, ct);

        // 5. Cập nhật total_frozen
        session.TotalFrozen += req.AmountDiamond;
        await _financeRepo.UpdateSessionAsync(session, ct);
        await _financeRepo.SaveChangesAsync(ct);

        return item.Id;
    }
}
