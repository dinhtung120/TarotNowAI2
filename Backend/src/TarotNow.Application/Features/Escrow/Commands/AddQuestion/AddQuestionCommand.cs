using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

/// <summary>
/// Command: User thêm câu hỏi — freeze thêm diamond, tạo question item mới.
/// Escrow cộng dồn: tổng frozen tăng.
/// </summary>
public class AddQuestionCommand : IRequest<Guid>
{
    public Guid UserId { get; set; }
    public string ConversationRef { get; set; } = string.Empty;
    public long AmountDiamond { get; set; }
    public string? ProposalMessageRef { get; set; }
    public string IdempotencyKey { get; set; } = string.Empty;
}

public class AddQuestionCommandHandler : IRequestHandler<AddQuestionCommand, Guid>
{
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IWalletRepository _walletRepo;

    public AddQuestionCommandHandler(IChatFinanceRepository financeRepo, IWalletRepository walletRepo)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
    }

    public async Task<Guid> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        // 1. Idempotency
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(req.IdempotencyKey, ct);
        if (existing != null) return existing.Id;

        // 2. Session phải tồn tại và active
        var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, ct)
            ?? throw new NotFoundException("Không tìm thấy phiên trò chuyện.");

        if (session.UserId != req.UserId)
            throw new BadRequestException("Bạn không phải chủ phiên.");

        if (session.Status != "active" && session.Status != "pending")
            throw new BadRequestException("Phiên đã kết thúc, không thể thêm câu hỏi.");

        // 3. Freeze thêm diamond
        await _walletRepo.FreezeAsync(
            req.UserId, req.AmountDiamond,
            referenceSource: "chat_question_item",
            referenceId: req.IdempotencyKey,
            description: $"Escrow add-question {req.AmountDiamond}💎",
            idempotencyKey: $"freeze_{req.IdempotencyKey}",
            cancellationToken: ct);

        // 4. Tạo question item (add_question)
        var now = DateTime.UtcNow;
        var item = new ChatQuestionItem
        {
            FinanceSessionId = session.Id,
            ConversationRef = req.ConversationRef,
            PayerId = req.UserId,
            ReceiverId = session.ReaderId,
            Type = QuestionItemType.AddQuestion,
            AmountDiamond = req.AmountDiamond,
            Status = QuestionItemStatus.Accepted,
            ProposalMessageRef = req.ProposalMessageRef,
            AcceptedAt = now,
            ReaderResponseDueAt = now.AddHours(24),
            AutoRefundAt = now.AddHours(24),
            IdempotencyKey = req.IdempotencyKey,
        };
        await _financeRepo.AddItemAsync(item, ct);

        // 5. Cộng dồn total_frozen
        session.TotalFrozen += req.AmountDiamond;
        await _financeRepo.UpdateSessionAsync(session, ct);
        await _financeRepo.SaveChangesAsync(ct);

        return item.Id;
    }
}
