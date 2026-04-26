using TarotNow.Application.Exceptions;
using TarotNow.Domain.Entities;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Escrow.Commands.AddQuestion;

public partial class AddQuestionCommandExecutor
{
    /// <summary>
    /// Validate và chuẩn hóa idempotency key cho luồng add-question.
    /// Luồng xử lý: trim key, chặn rỗng và giới hạn độ dài 128 ký tự.
    /// </summary>
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

    /// <summary>
    /// Thực thi workflow add-question trong transaction.
    /// Luồng xử lý: tải session hợp lệ, freeze kim cương, tạo question item, cập nhật tổng frozen của session và lưu thay đổi.
    /// </summary>
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
            await _domainEventPublisher.PublishAsync(
                new MoneyChangedDomainEvent
                {
                    UserId = request.UserId,
                    Currency = CurrencyType.Diamond,
                    ChangeType = TransactionType.EscrowFreeze,
                    DeltaAmount = -request.AmountDiamond,
                    ReferenceId = idempotencyKey
                },
                transactionCt);

            var item = BuildAddQuestionItem(
                request,
                session.ReaderId,
                session.Id,
                idempotencyKey);
            await _financeRepo.AddItemAsync(item, transactionCt);

            // Cập nhật tổng frozen sau khi thêm item mới để session phản ánh đúng số dư đang giữ.
            session.TotalFrozen += request.AmountDiamond;
            await _financeRepo.UpdateSessionAsync(session, transactionCt);
            await _financeRepo.SaveChangesAsync(transactionCt);
            createdItemId = item.Id;
        }, cancellationToken);

        return createdItemId;
    }

    /// <summary>
    /// Tải session và validate quyền/thuộc tính cho thao tác add-question.
    /// Luồng xử lý: lấy session theo conversation ref, kiểm tra owner user và trạng thái session cho phép.
    /// </summary>
    private async Task<ChatFinanceSession> LoadValidatedSessionAsync(
        AddQuestionCommand request,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(request.ConversationRef, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy phiên trò chuyện.");

        if (session.UserId != request.UserId)
        {
            // Chỉ chủ session (payer) mới được thêm câu hỏi phát sinh.
            throw new BadRequestException("Bạn không phải chủ phiên.");
        }

        if (session.Status != ChatFinanceSessionStatus.Active
            && session.Status != ChatFinanceSessionStatus.Pending)
        {
            // Session đã kết thúc/không hợp lệ thì không nhận thêm câu hỏi.
            throw new BadRequestException("Phiên đã kết thúc, không thể thêm câu hỏi.");
        }

        return session;
    }

    /// <summary>
    /// Freeze số kim cương cho câu hỏi phát sinh.
    /// Luồng xử lý: gọi wallet freeze với reference/idempotency theo key đã chuẩn hóa.
    /// </summary>
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

    /// <summary>
    /// Dựng entity ChatQuestionItem cho thao tác add-question.
    /// Luồng xử lý: map dữ liệu request, gán trạng thái Accepted và thiết lập các mốc thời gian xử lý/autorefund.
    /// </summary>
    private ChatQuestionItem BuildAddQuestionItem(
        AddQuestionCommand request,
        Guid readerId,
        Guid sessionId,
        string idempotencyKey)
    {
        var now = DateTime.UtcNow;
        var normalizedReaderResponseDueHours = _systemConfigSettings.EscrowReaderResponseDueHours;
        var normalizedAutoRefundHours = _systemConfigSettings.EscrowAutoRefundHours;

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
            ReaderResponseDueAt = now.AddHours(normalizedReaderResponseDueHours),
            AutoRefundAt = now.AddHours(normalizedAutoRefundHours),
            IdempotencyKey = idempotencyKey
        };
    }
}
