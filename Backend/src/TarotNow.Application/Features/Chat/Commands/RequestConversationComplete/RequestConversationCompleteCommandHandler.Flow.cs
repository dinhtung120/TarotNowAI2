using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandHandlerRequestedDomainEventHandler
{
    /// <summary>
    /// Xử lý nhánh first-request complete nếu đây là lần yêu cầu đầu tiên.
    /// Luồng xử lý: nếu không phải first request thì bỏ qua; nếu phải thì hủy pending add-money, ghi metadata confirm và thêm system message.
    /// </summary>
    private async Task<DateTime?> HandleFirstRequestIfNeededAsync(
        RequestContext context,
        CancellationToken cancellationToken)
    {
        if (context.IsFirstRequest == false)
        {
            // Không phải lần đầu thì không cần set requestedBy/requestedAt/autoResolveAt.
            return null;
        }

        // Hủy offer cộng tiền pending để tránh xung đột với flow hoàn thành.
        var lastMessageAt = await CancelPendingAddMoneyOfferAsync(
            context.Conversation,
            context.RequesterId,
            cancellationToken);

        context.Conversation.Confirm!.RequestedBy = context.RequesterId;
        context.Conversation.Confirm.RequestedAt = context.Now;
        context.Conversation.Confirm.AutoResolveAt = context.Now.AddHours(context.IsUserRequester ? 12 : 48);

        return await AddSystemMessageAsync(
            context.Conversation,
            context.RequesterId,
            new SystemMessageSpec(
                ChatMessageType.System,
                BuildFirstRequestMessage(context.IsUserRequester),
                lastMessageAt),
            cancellationToken);
    }

    /// <summary>
    /// Dựng nội dung system message cho lần yêu cầu complete đầu tiên.
    /// Luồng xử lý: chọn template theo vai trò requester (user hoặc reader).
    /// </summary>
    private static string BuildFirstRequestMessage(bool isUserRequester)
    {
        return isUserRequester
            ? "User đã yêu cầu hoàn thành cuộc trò chuyện. Nếu Reader không phản hồi, hệ thống sẽ tự động giải ngân sau tối đa 12 giờ."
            : "Reader đã đánh dấu hoàn thành. Nếu User không phản hồi, hệ thống sẽ tự động giải ngân sau tối đa 48 giờ.";
    }

    /// <summary>
    /// Ghi dấu xác nhận complete cho bên requester hiện tại.
    /// Luồng xử lý: cập nhật UserAt hoặc ReaderAt theo vai trò requester.
    /// </summary>
    private static void ApplyRequesterConfirmation(RequestContext context)
    {
        if (context.IsUserRequester)
        {
            // Requester là user: ghi mốc xác nhận phía user.
            context.Conversation.Confirm!.UserAt = context.Now;
            return;
        }

        // Requester là reader: ghi mốc xác nhận phía reader.
        context.Conversation.Confirm!.ReaderAt = context.Now;
    }

    /// <summary>
    /// Kiểm tra conversation đã đủ xác nhận từ cả user và reader chưa.
    /// Luồng xử lý: xác nhận cả UserAt và ReaderAt đều có giá trị.
    /// </summary>
    private static bool HasBothSidesConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }

    /// <summary>
    /// Lưu trạng thái conversation khi đang chờ bên còn lại xác nhận complete.
    /// Luồng xử lý: cập nhật updatedAt, cập nhật lastMessageAt nếu có message mới, rồi persist.
    /// </summary>
    private async Task PersistPendingConversationAsync(
        ConversationDto conversation,
        DateTime now,
        DateTime? lastMessageAt,
        CancellationToken cancellationToken)
    {
        conversation.UpdatedAt = now;
        if (lastMessageAt.HasValue)
        {
            // Chỉ ghi đè LastMessageAt khi có message hệ thống mới phát sinh.
            conversation.LastMessageAt = lastMessageAt.Value;
        }

        await _conversationRepository.UpdateAsync(conversation, cancellationToken);
    }
}
