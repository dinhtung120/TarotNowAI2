/*
 * ===================================================================
 * FILE: MarkMessagesReadCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.MarkMessagesRead
 * ===================================================================
 * MỤC ĐÍCH:
 *   Thông báo cho hệ thống biết "Tôi đã xem qua các tin nhắn mới".
 *   Chức năng này chứa cả Command và Handler.
 *   
 * ỨNG DỤNG THỰC TẾ:
 *   Khi User nhấp vào Box Chat -> Gọi API này -> MongoDB sẽ:
 *   1. Chuyển trạng thái `IsRead = true` cho tất cả tin nhắn của đối phương gửi.
 *   2. Đặt `UnreadCount` (Số tin nhắn chưa đọc màu đỏ bốc lửa trên màn hình) về 0.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

/// <summary>
/// Gói lệnh kích hoạt sự kiện "Đã Nhìn Thấy Tin Nhắn" (Seen).
/// </summary>
public class MarkMessagesReadCommand : IRequest<bool>
{
    /// <summary>Mã ID của Box Chat trên MongoDB.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>Người đang dán mắt vào Box Chat (Có thể là User hoặc Reader).</summary>
    public Guid ReaderId { get; set; }
}

/// <summary>
/// Handler thi hành nghiệp vụ xóa chấm đỏ Unread.
/// </summary>
public class MarkMessagesReadCommandHandler : IRequestHandler<MarkMessagesReadCommand, bool>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public MarkMessagesReadCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<bool> Handle(MarkMessagesReadCommand request, CancellationToken cancellationToken)
    {
        // 1. Trích xuất Box Chat
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var readerId = request.ReaderId.ToString();

        // 2. An Ninh: Tấm chắn người ngoài vọng vào.
        // Chỉ có chủ thể hoặc người thợ xem bài (2 người trong Box) mới được phép "Seen".
        if (conversation.UserId != readerId && conversation.ReaderId != readerId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 3. Quét tất cả tin nhắn CỦA ĐỐI PHƯƠNG đang ở trạng thái IsRead=False -> Đổi thành True.
        await _messageRepo.MarkAsReadAsync(request.ConversationId, readerId, cancellationToken);

        // 4. Xóa chấm đỏ báo cáo ở Ngoài Màn Hình Danh Sách Chat:
        // Ai đang nắm Request này thì Số đếm (UnreadCount) của người đó sẽ Reset về 0.
        if (readerId == conversation.UserId)
            conversation.UnreadCountUser = 0;
        else
            conversation.UnreadCountReader = 0;

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        return true;
    }
}
