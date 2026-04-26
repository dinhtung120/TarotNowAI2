using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandExecutor
{
    /// <summary>
    /// Tạo question item pending đại diện cho khoản freeze câu hỏi chính.
    /// Luồng xử lý: map context freeze vào ChatQuestionItem và lưu vào kho dữ liệu tài chính.
    /// </summary>
    private async Task AddPendingMainQuestionAsync(
        Guid sessionId,
        string conversationId,
        MainQuestionFreezeContext context,
        CancellationToken cancellationToken)
    {
        var now = DateTime.UtcNow;
        await _financeRepo.AddItemAsync(new Domain.Entities.ChatQuestionItem
        {
            FinanceSessionId = sessionId,
            ConversationRef = conversationId,
            PayerId = context.UserId,
            ReceiverId = context.ReaderId,
            Type = QuestionItemType.MainQuestion,
            AmountDiamond = context.AmountDiamond,
            Status = QuestionItemStatus.Pending,
            OfferExpiresAt = context.OfferExpiresAtUtc,
            IdempotencyKey = context.IdempotencyKey,
            CreatedAt = now,
            UpdatedAt = now
        }, cancellationToken);
    }

    /// <summary>
    /// Cập nhật tổng số dư frozen của session sau khi thêm item pending.
    /// Luồng xử lý: cộng dồn TotalFrozen, giữ trạng thái pending, rồi persist session và save changes.
    /// </summary>
    private async Task UpdatePendingSessionFrozenAmountAsync(
        Domain.Entities.ChatFinanceSession session,
        long amountDiamond,
        CancellationToken cancellationToken)
    {
        session.TotalFrozen += amountDiamond;
        session.Status = ChatFinanceSessionStatus.Pending;
        session.UpdatedAt = DateTime.UtcNow;

        await _financeRepo.UpdateSessionAsync(session, cancellationToken);
        // Lưu ngay để số dư frozen nhất quán với item vừa tạo.
        await _financeRepo.SaveChangesAsync(cancellationToken);
    }
}
