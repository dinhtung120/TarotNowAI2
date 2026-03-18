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
    private readonly ITransactionCoordinator _transactionCoordinator;

    public AcceptOfferCommandHandler(
        IChatFinanceRepository financeRepo,
        IWalletRepository walletRepo,
        ITransactionCoordinator transactionCoordinator)
    {
        _financeRepo = financeRepo;
        _walletRepo = walletRepo;
        _transactionCoordinator = transactionCoordinator;
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

        Guid createdItemId = Guid.Empty;
        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            // 2. Lấy hoặc tạo finance session
            var session = await _financeRepo.GetSessionByConversationRefAsync(req.ConversationRef, transactionCt);
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
                await _financeRepo.AddSessionAsync(session, transactionCt);
            }

            // 3. Freeze diamond + persist metadata trong cùng transaction
            await _walletRepo.FreezeAsync(
                req.UserId, req.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: idempotencyKey,
                description: $"Escrow freeze {req.AmountDiamond}💎 cho conversation {req.ConversationRef}",
                idempotencyKey: $"freeze_{idempotencyKey}",
                cancellationToken: transactionCt);

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
            await _financeRepo.AddItemAsync(item, transactionCt);

            session.TotalFrozen += req.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);

            createdItemId = item.Id;
        }, ct);

        return createdItemId;
    }
}
