using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

public partial class AddQuestionCommandHandler
{
    private static string ValidateIdempotencyKey(string? idempotencyKey)
    {
        var normalized = idempotencyKey?.Trim();
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new BadRequestException("IdempotencyKey là bắt buộc.");
        }

        if (normalized.Length > 128)
        {
            throw new BadRequestException("IdempotencyKey quá dài (tối đa 128 ký tự).");
        }

        return normalized;
    }

    private async Task<Guid> ExecuteAddQuestionAsync(
        AddQuestionCommand request,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        Guid createdItemId = Guid.Empty;

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var session = await LoadValidatedSessionAsync(request, transactionCt);
            await FreezeQuestionAmountAsync(request, idempotencyKey, transactionCt);

            var item = BuildAddQuestionItem(request, session.ReaderId, session.Id, idempotencyKey);
            await _financeRepo.AddItemAsync(item, transactionCt);

            session.TotalFrozen += request.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);
            createdItemId = item.Id;
        }, cancellationToken);

        return createdItemId;
    }

    private async Task<ChatFinanceSession> LoadValidatedSessionAsync(
        AddQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(request.ConversationRef, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy phiên trò chuyện.");

        if (session.UserId != request.UserId)
        {
            throw new BadRequestException("Bạn không phải chủ phiên.");
        }

        if (session.Status != "active" && session.Status != "pending")
        {
            throw new BadRequestException("Phiên đã kết thúc, không thể thêm câu hỏi.");
        }

        return session;
    }

    private async Task FreezeQuestionAmountAsync(
        AddQuestionCommand request,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        await _walletRepo.FreezeAsync(
            request.UserId,
            request.AmountDiamond,
            referenceSource: "chat_question_item",
            referenceId: idempotencyKey,
            description: $"Escrow add-question {request.AmountDiamond}💎",
            idempotencyKey: $"freeze_{idempotencyKey}",
            cancellationToken: cancellationToken);
    }

    private static ChatQuestionItem BuildAddQuestionItem(
        AddQuestionCommand request,
        Guid readerId,
        Guid sessionId,
        string idempotencyKey)
    {
        var now = DateTime.UtcNow;
        return new ChatQuestionItem
        {
            FinanceSessionId = sessionId,
            ConversationRef = request.ConversationRef,
            PayerId = request.UserId,
            ReceiverId = readerId,
            Type = QuestionItemType.AddQuestion,
            AmountDiamond = request.AmountDiamond,
            Status = QuestionItemStatus.Accepted,
            ProposalMessageRef = request.ProposalMessageRef,
            AcceptedAt = now,
            ReaderResponseDueAt = now.AddHours(24),
            AutoRefundAt = now.AddHours(24),
            IdempotencyKey = idempotencyKey
        };
    }
}
