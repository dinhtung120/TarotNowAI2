using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RespondConversationComplete;

public partial class RespondConversationCompleteCommandExecutor
{
    // Gói context cho luồng phản hồi complete để tái sử dụng dữ liệu đã validate.
    private readonly record struct ResponseContext(
        ConversationDto Conversation,
        string RequesterId,
        DateTime Now);

    /// <summary>
    /// Dựng context xử lý cho phản hồi yêu cầu hoàn thành.
    /// Luồng xử lý: tải conversation, kiểm tra quyền participant, kiểm tra trạng thái và điều kiện có pending complete request.
    /// </summary>
    private async Task<ResponseContext> BuildContextAsync(
        RespondConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            // Chỉ participant của conversation mới có quyền phản hồi complete request.
            throw new BadRequestException("Bạn không thuộc cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            // Chỉ phản hồi được khi conversation còn Ongoing.
            throw new BadRequestException($"Không thể phản hồi hoàn thành ở trạng thái '{conversation.Status}'.");
        }

        if (conversation.Confirm == null || string.IsNullOrWhiteSpace(conversation.Confirm.RequestedBy))
        {
            // Edge case: không có pending complete request để phản hồi.
            throw new BadRequestException("Không có yêu cầu hoàn thành nào đang chờ phản hồi.");
        }

        if (string.Equals(conversation.Confirm.RequestedBy, requesterId, StringComparison.Ordinal))
        {
            // Người khởi tạo request không được tự chấp thuận/từ chối yêu cầu của chính mình.
            throw new BadRequestException("Bạn là người đã gửi yêu cầu hoàn thành, không thể tự phản hồi.");
        }

        return new ResponseContext(conversation, requesterId, DateTime.UtcNow);
    }

    /// <summary>
    /// Ghi mốc xác nhận của bên responder vào conversation confirm state.
    /// Luồng xử lý: cập nhật UserAt hoặc ReaderAt tương ứng theo vai trò participant.
    /// </summary>
    private static void ApplyResponderConfirmation(ResponseContext context)
    {
        if (context.Conversation.UserId == context.RequesterId)
        {
            // Responder là user nên ghi mốc UserAt.
            context.Conversation.Confirm!.UserAt = context.Now;
            return;
        }

        // Responder là reader nên ghi mốc ReaderAt.
        context.Conversation.Confirm!.ReaderAt = context.Now;
    }

    /// <summary>
    /// Kiểm tra conversation đã đủ xác nhận từ cả user và reader hay chưa.
    /// Luồng xử lý: xác định hai mốc confirm đều khác null.
    /// </summary>
    private static bool HasBothSidesConfirmed(ConversationDto conversation)
    {
        return conversation.Confirm?.UserAt != null && conversation.Confirm.ReaderAt != null;
    }
}
