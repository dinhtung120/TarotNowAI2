using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.RequestConversationComplete;

public partial class RequestConversationCompleteCommandExecutor
{
    // Gói context để các bước xử lý dùng chung dữ liệu đã validate, tránh đọc DB lặp lại.
    private readonly record struct RequestContext(
        ConversationDto Conversation,
        string RequesterId,
        bool IsUserRequester,
        bool IsFirstRequest,
        DateTime? RequesterConfirmedAt,
        DateTime? ResponderConfirmedAt,
        DateTime Now);

    /// <summary>
    /// Dựng context xử lý từ command đầu vào và trạng thái conversation hiện tại.
    /// Luồng xử lý: tải conversation, kiểm tra quyền tham gia và trạng thái hợp lệ, rồi xác định vai trò requester cùng các mốc confirm liên quan.
    /// </summary>
    private async Task<RequestContext> BuildContextAsync(
        RequestConversationCompleteCommand request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            // Business rule: chỉ participant thuộc conversation mới được yêu cầu hoàn thành.
            throw new BadRequestException("Bạn không thuộc cuộc trò chuyện này.");
        }

        if (conversation.Status != ConversationStatus.Ongoing)
        {
            // Chỉ cho phép thao tác complete khi conversation đang Ongoing.
            throw new BadRequestException($"Không thể yêu cầu hoàn thành ở trạng thái '{conversation.Status}'.");
        }

        var now = DateTime.UtcNow;
        // Khởi tạo confirm object nếu thiếu để tránh null-reference ở các bước cập nhật mốc thời gian.
        conversation.Confirm ??= new ConversationConfirmDto();

        var isUserRequester = conversation.UserId == requesterId;
        var requesterConfirmedAt = isUserRequester ? conversation.Confirm.UserAt : conversation.Confirm.ReaderAt;
        var responderConfirmedAt = isUserRequester ? conversation.Confirm.ReaderAt : conversation.Confirm.UserAt;
        var isFirstRequest = conversation.Confirm.UserAt == null && conversation.Confirm.ReaderAt == null;

        return new RequestContext(
            conversation,
            requesterId,
            isUserRequester,
            isFirstRequest,
            requesterConfirmedAt,
            responderConfirmedAt,
            now);
    }

    /// <summary>
    /// Kiểm tra requester đã xác nhận trước đó nhưng phía còn lại chưa xác nhận hay chưa.
    /// Luồng xử lý: dựa vào hai mốc confirm trong context để quyết định bỏ qua side-effect lặp.
    /// </summary>
    private static bool IsAlreadyConfirmedByRequester(RequestContext context)
    {
        return context.RequesterConfirmedAt != null && context.ResponderConfirmedAt == null;
    }

}
