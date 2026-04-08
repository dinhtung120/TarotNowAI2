using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandHandler
{
    /// <summary>
    /// Xử lý nhánh từ chối yêu cầu hoàn thành.
    /// Luồng xử lý: ghi system message từ chối, xóa trạng thái confirm pending, cập nhật conversation và trả kết quả rejected.
    /// </summary>
    private async Task<ConversationCompleteRespondResult> RejectCompletionRequestAsync(
        ResponseContext context,
        CancellationToken cancellationToken)
    {
        var rejectMessage = BuildRejectMessage(context.Conversation);
        var rejectAt = await AddSystemMessageAsync(
            context.Conversation,
            context.RequesterId,
            new SystemMessageSpec(ChatMessageType.System, rejectMessage, DateTime.UtcNow),
            cancellationToken);

        context.Conversation.Confirm = null;
        context.Conversation.UpdatedAt = rejectAt;
        context.Conversation.LastMessageAt = rejectAt;
        // Reset trạng thái confirm để conversation quay lại luồng ongoing bình thường.
        await _conversationRepository.UpdateAsync(context.Conversation, cancellationToken);

        return new ConversationCompleteRespondResult
        {
            Status = context.Conversation.Status,
            Accepted = false
        };
    }

    /// <summary>
    /// Dựng nội dung message khi yêu cầu hoàn thành bị từ chối.
    /// Luồng xử lý: phân biệt thông điệp theo phía đã gửi yêu cầu ban đầu để người nhận hiểu ngữ cảnh.
    /// </summary>
    private static string BuildRejectMessage(ConversationDto conversation)
    {
        return conversation.Confirm?.RequestedBy == conversation.ReaderId
            ? "User đã từ chối yêu cầu hoàn thành của Reader. Bạn có thể mở tranh chấp."
            : "Reader đã từ chối yêu cầu hoàn thành của User.";
    }
}
