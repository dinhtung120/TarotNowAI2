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
    private readonly ITransactionCoordinator _transactionCoordinator;

    public AddQuestionCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
    }

    public async Task<Guid> Handle(AddQuestionCommand req, CancellationToken ct)
    {
        var idempotencyKey = req.IdempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(idempotencyKey))
            throw new BadRequestException("IdempotencyKey là bắt buộc.");

        if (idempotencyKey.Length > 128)
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");

        // 1. Idempotency
        var existing = await _financeRepo.GetItemByIdempotencyKeyAsync(idempotencyKey, ct);
        if (existing != null) return existing.Id;

        Guid createdItemId = Guid.Empty;
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // 2. Session phải tồn tại và active
            var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, transactionCt)
                ?? throw new NotFoundException("Không tìm thấy phiên trò chuyện.");

            if (session.UserId != req.UserId)
                throw new BadRequestException("Bạn không phải chủ phiên.");

            if (session.Status != "active" && session.Status != "pending")
                throw new BadRequestException("Phiên đã kết thúc, không thể thêm câu hỏi.");

            // 3. Freeze + persist metadata trong cùng transaction
            await _walletRepo.FreezeAsync(
                req.UserId, req.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: idempotencyKey,
                description: $"Escrow add-question {req.AmountDiamond}💎",
                idempotencyKey: $"freeze_{idempotencyKey}",
                cancellationToken: transactionCt);

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
                IdempotencyKey = idempotencyKey,
            };
            await _financeRepo.AddItemAsync(item, transactionCt);

            session.TotalFrozen += req.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);

            createdItemId = item.Id;
        }, ct);

        return createdItemId;
    }
}
