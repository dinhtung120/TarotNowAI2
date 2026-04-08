using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandler
{
    /// <summary>
    /// Thực thi luồng freeze câu hỏi chính trong transaction.
    /// Luồng xử lý: kiểm tra idempotency, lấy/tạo session pending, freeze ví, thêm question item pending và cập nhật tổng frozen.
    /// </summary>
    private async Task FreezeMainQuestionAsync(
        ConversationDto conversation,
        MainQuestionFreezeContext context,
        CancellationToken cancellationToken)
    {
        if (await _financeRepo.GetItemByIdempotencyKeyAsync(context.IdempotencyKey, cancellationToken) != null)
        {
            // Idempotency: nếu item đã tồn tại thì bỏ qua để tránh freeze trùng.
            return;
        }

        var session = await GetOrCreatePendingSessionAsync(
            conversation,
            context.UserId,
            context.ReaderId,
            cancellationToken);
        // Thực hiện đóng băng số dư trước khi ghi item tài chính để đảm bảo thứ tự nghiệp vụ.
        await FreezeWalletAsync(
            conversation.Id,
            context.UserId,
            context.AmountDiamond,
            context.IdempotencyKey,
            cancellationToken);
        await AddPendingMainQuestionAsync(session.Id, conversation.Id, context, cancellationToken);
        await UpdatePendingSessionFrozenAmountAsync(session, context.AmountDiamond, cancellationToken);
    }

    /// <summary>
    /// Lấy session tài chính hiện hữu hoặc tạo mới session pending cho conversation.
    /// Luồng xử lý: ưu tiên session đã có; nếu chưa có thì tạo session mới với tổng frozen ban đầu bằng 0.
    /// </summary>
    private async Task<Domain.Entities.ChatFinanceSession> GetOrCreatePendingSessionAsync(
        ConversationDto conversation,
        Guid userId,
        Guid readerId,
        CancellationToken cancellationToken)
    {
        var session = await _financeRepo.GetSessionByConversationRefAsync(conversation.Id, cancellationToken);
        if (session != null)
        {
            // Tái sử dụng session hiện tại để gom các item cùng conversation.
            return session;
        }

        session = new Domain.Entities.ChatFinanceSession
        {
            ConversationRef = conversation.Id,
            UserId = userId,
            ReaderId = readerId,
            Status = "pending",
            TotalFrozen = 0
        };

        await _financeRepo.AddSessionAsync(session, cancellationToken);
        return session;
    }

    /// <summary>
    /// Gửi lệnh đóng băng kim cương từ ví user cho câu hỏi chính.
    /// Luồng xử lý: gọi wallet freeze với idempotency key ổn định theo conversation.
    /// </summary>
    private Task FreezeWalletAsync(
        string conversationId,
        Guid userId,
        long amountDiamond,
        string idempotencyKey,
        CancellationToken cancellationToken)
    {
        return _walletRepo.FreezeAsync(
            userId,
            amountDiamond,
            referenceSource: "chat_question_item",
            referenceId: idempotencyKey,
            description: $"Escrow freeze {amountDiamond}💎 cho conversation {conversationId}",
            idempotencyKey: $"freeze_{idempotencyKey}",
            cancellationToken: cancellationToken);
    }
}
