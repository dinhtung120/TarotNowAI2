/*
 * ===================================================================
 * FILE: ListMessagesQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Queries.ListMessages
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh truy xuất Lịch Sử Tin Nhắn bên trong một Box Chat cụ thể.
 *   (Dùng khi bấm vào 1 dòng chat trên màn hình Inbox để mở giao diện nhắn tin).
 *
 * CHIẾN LƯỢC UI:
 *   Backend luôn trả về dữ liệu sắp xếp theo Thời gian Mới Nhất (DESC) 
 *   để phân trang. Khi đổ lên Frontend, điện thoại sẽ tự Reverse Lại 
 *   rồi ép cuộn xuống đáy giống iMessage/Zalo.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

/// <summary>
/// Yêu cầu chiết xuất lịch sử hội thoại cho mục đích hiển thị Màn Hình Chat.
/// </summary>
public class ListMessagesQuery : IRequest<ListMessagesResult>
{
    /// <summary>Khoá chính của Cuộc trò chuyện cần lục lịch sử.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>Ai đang yêu cầu xem lịch sử này? (Dùng làm lá chắn bảo mật tránh nhìn lén).</summary>
    public Guid RequesterId { get; set; }

    /// <summary>
    /// Cursor message id (ObjectId string). Null = trang đầu (tin mới nhất).
    /// </summary>
    public string? Cursor { get; set; }

    /// <summary>
    /// Số tin nhắn tối đa mỗi lần gọi.
    /// </summary>
    public int Limit { get; set; } = 50;
}

public class ListMessagesResult
{
    public IReadOnlyList<ChatMessageDto> Messages { get; set; } = Array.Empty<ChatMessageDto>();
    public string? NextCursor { get; set; }
    
    // [YÊU CẦU MỚI]: Đính kèm thông tin Header cho Room Chat
    public ConversationDto Conversation { get; set; } = null!;
}
