using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Domain.Events;
using System.Linq;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

public partial class SendMessageCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Thực thi luồng freeze câu hỏi chính trong transaction.
    /// Luồng xử lý: kiểm tra idempotency, lấy/tạo session pending, freeze ví, thêm question item pending và cập nhật tổng frozen.
    /// </summary>
    private async Task<MainQuestionFreezeExecutionResult> FreezeMainQuestionAsync(
        ConversationDto conversation,
        MainQuestionFreezeContext context,
        CancellationToken cancellationToken)
    {
        var existingItem = await _financeRepo.GetItemByIdempotencyKeyAsync(context.IdempotencyKey, cancellationToken);
        if (existingItem != null)
        {
            // Idempotency: nếu item đã tồn tại thì bỏ qua để tránh freeze trùng.
            return MainQuestionFreezeExecutionResult.Skipped(
                context.OfferExpiresAtUtc,
                context.IdempotencyKey);
        }

        var session = await GetOrCreatePendingSessionAsync(
            conversation,
            context.UserId,
            context.ReaderId,
            cancellationToken);
        var activeMainQuestion = await GetActiveMainQuestionItemAsync(session.Id, cancellationToken);
        if (activeMainQuestion != null)
        {
            // Conversation đã có khoản freeze active từ attempt khác thì không freeze thêm.
            return MainQuestionFreezeExecutionResult.Skipped(
                activeMainQuestion.OfferExpiresAt ?? context.OfferExpiresAtUtc,
                activeMainQuestion.IdempotencyKey ?? context.IdempotencyKey);
        }

        // Thực hiện đóng băng số dư trước khi ghi item tài chính để đảm bảo thứ tự nghiệp vụ.
        await FreezeWalletAsync(
            conversation.Id,
            context.UserId,
            context.AmountDiamond,
            context.IdempotencyKey,
            cancellationToken);
        await AddPendingMainQuestionAsync(session.Id, conversation.Id, context, cancellationToken);
        await UpdatePendingSessionFrozenAmountAsync(session, context.AmountDiamond, cancellationToken);
        return MainQuestionFreezeExecutionResult.Applied(
            context.OfferExpiresAtUtc,
            context.IdempotencyKey);
    }

    private async Task<Domain.Entities.ChatQuestionItem?> GetActiveMainQuestionItemAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var items = await _financeRepo.GetItemsBySessionIdAsync(sessionId, cancellationToken);
        return items.FirstOrDefault(static item =>
            item.Type == QuestionItemType.MainQuestion
            && item.Status is QuestionItemStatus.Pending or QuestionItemStatus.Accepted or QuestionItemStatus.Disputed);
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

    /// <summary>
    /// Bù trừ khoản freeze câu hỏi chính khi lưu message/conversation thất bại sau bước freeze.
    /// Luồng xử lý: refund ví theo idempotency ổn định, cập nhật item/session về trạng thái refunded và phát event hoàn tiền.
    /// </summary>
    private async Task CompensateMainQuestionFreezeAsync(
        ConversationDto conversation,
        Guid senderId,
        string freezeItemIdempotencyKey,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(freezeItemIdempotencyKey))
        {
            return;
        }

        await _transactionCoordinator.ExecuteAsync(async transactionCt =>
        {
            var item = await _financeRepo.GetItemByIdempotencyKeyAsync(freezeItemIdempotencyKey, transactionCt);
            if (item == null || item.Status != QuestionItemStatus.Pending)
            {
                return;
            }

            await _walletRepo.RefundAsync(
                senderId,
                item.AmountDiamond,
                referenceSource: "chat_question_item",
                referenceId: item.Id.ToString(),
                description: $"Rollback escrow freeze for failed first message in conversation {conversation.Id}",
                idempotencyKey: $"rollback_{freezeItemIdempotencyKey}",
                cancellationToken: transactionCt);

            var now = DateTime.UtcNow;
            item.Status = QuestionItemStatus.Refunded;
            item.RefundedAt = now;
            item.UpdatedAt = now;
            await _financeRepo.UpdateItemAsync(item, transactionCt);

            var session = await _financeRepo.GetSessionForUpdateAsync(item.FinanceSessionId, transactionCt);
            if (session != null)
            {
                session.TotalFrozen = Math.Max(0, session.TotalFrozen - item.AmountDiamond);
                session.Status = session.TotalFrozen > 0 ? ChatFinanceSessionStatus.Pending : ChatFinanceSessionStatus.Refunded;
                session.UpdatedAt = now;
                await _financeRepo.UpdateSessionAsync(session, transactionCt);
            }

            await _domainEventPublisher.PublishAsync(
                new EscrowRefundedDomainEvent
                {
                    ItemId = item.Id,
                    UserId = item.PayerId,
                    AmountDiamond = item.AmountDiamond,
                    RefundSource = "send_message_failed_compensation"
                },
                transactionCt);

            await _financeRepo.SaveChangesAsync(transactionCt);
        }, cancellationToken);
    }
}
