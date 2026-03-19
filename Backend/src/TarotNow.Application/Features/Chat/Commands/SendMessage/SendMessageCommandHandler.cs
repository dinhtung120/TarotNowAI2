/*
 * ===================================================================
 * FILE: SendMessageCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.SendMessage
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý Cốt Lõi Hệ Thống Gửi Tin.
 * 
 * PHÂN CHIA TRÁCH NHIỆM (CLEAN ARCHITECTURE):
 *   Handler này CHỈ LƯU VÀO DATABASE (Cập nhật MongoDB).
 *   Tại sao ở đây Không xài lệnh SignalR.Clients.All.SendAsync() bắn đi liền?
 *   => Vì làm thế sẽ phá vỡ quy tắc SOLID. Handler Command không nên phụ thuộc 
 *   vào Tầng Transport (SignalR/WebSocket). SignalR Hub sẽ gọi Handler này, 
 *   lấy kết quả Data, rồi TỰ SIGNALR Hub mới lo việc Broadcast (phát sóng).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

/// <summary>
/// Xử lý logic nghiệp vụ ghi lưu Tin nhắn, đẩy đếm số chấm đỏ UnreadCount.
/// </summary>
public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand, ChatMessageDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public SendMessageCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<ChatMessageDto> Handle(SendMessageCommand request, CancellationToken cancellationToken)
    {
        // 1. Kiểm duyệt Định dạng: Loại tin lạ không thuộc hệ sinh thái sẽ bị ném ra ngoài.
        if (!ChatMessageType.IsValid(request.Type))
            throw new BadRequestException($"Loại tin nhắn không hợp lệ: {request.Type}");

        // 2. Chặn gửi tin nhắn trống trơn (Nhỡ User bấm nút Enter liên tục).
        if (request.Type == ChatMessageType.Text && string.IsNullOrWhiteSpace(request.Content))
            throw new BadRequestException("Nội dung tin nhắn không được để trống.");

        // 3. Truy vết Box Chat.
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // 4. An Ninh: Chỉ thành viên phòng Chat mới được nói chuyện.
        var senderId = request.SenderId.ToString();
        if (conversation.UserId != senderId && conversation.ReaderId != senderId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 5. Kiểm tra Trạng Thái: Hợp đồng kết thúc (Ended/Closed) thì không thể nhắn gửi nhung nhớ gì nữa.
        if (conversation.Status != ConversationStatus.Active && conversation.Status != ConversationStatus.Pending)
            throw new BadRequestException($"Cuộc trò chuyện đã kết thúc ({conversation.Status}).");

        // 6. Trigger Cờ Tự Động: 
        // Lần đầu tiên Reader Rep lại hộp thoại Pending -> Room sẽ tự nhảy sang Active (Hai bên chính thức bắt sóng).
        if (conversation.Status == ConversationStatus.Pending)
        {
            conversation.Status = ConversationStatus.Active;
        }

        // 7. Sinh Nở Đối Tượng DTO Tin Nhắn mới.
        var message = new ChatMessageDto
        {
            ConversationId = request.ConversationId,
            SenderId = senderId,
            Type = request.Type,
            Content = request.Content,
            PaymentPayload = request.PaymentPayload,
            IsRead = false, // Vừa ra lò thì dĩ nhiên chưa ai đọc (Chấm Đỏ Kích Hoạt)
            CreatedAt = DateTime.UtcNow
        };

        // 8. Đẩy vào lò nung Database (Collection: chat_messages).
        await _messageRepo.AddAsync(message, cancellationToken);

        // 9. CẬP NHẬT TRẠNG THÁI BOX CHAT BÊN NGOÀI BẢNG (Collection: conversations).
        // Phải cập nhật thời gian để Frontend kéo Box Chat lên Đầu Danh Sách.
        conversation.LastMessageAt = message.CreatedAt;
        conversation.UpdatedAt = message.CreatedAt;

        // 10. TÍNH TOÁN CHẤM ĐỎ UNREAD THÔNG MINH
        // Nếu BẠN là người gửi -> Đối Phương sẽ bị tăng Bộ đếm Unread.
        if (senderId == conversation.UserId)
            conversation.UnreadCountReader += 1;
        else
            conversation.UnreadCountUser += 1;

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        // 11. Trả về DTO rực rỡ để SignalR cầm đi rải mạng (Broadcast).
        return message;
    }
}
