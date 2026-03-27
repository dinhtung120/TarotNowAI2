/*
 * ===================================================================
 * FILE: CreateConversationCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.CreateConversation
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói cấu trúc dữ liệu cho Yêu cầu Khởi tạo Phòng Chat 1-1 giữa User và Reader.
 *   (Dùng MongoDB document schema).
 *   
 * QUY TẮC NGHIỆP VỤ CƠ BẢN:
 *   - User và Reader phải là 2 người khác nhau.
 *   - Reader phải đang bấm "Online nhận khách mới/Accepting Questions".
 *   - Box Chat luôn ra đời với trạng thái Pending (Chưa phản hồi).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

/// <summary>
/// Command kích hoạt xây dựng Document Chat trên MongoDB.
/// Trả về DTO Conversation chứa dữ liệu Box Chat vừa tạo.
/// </summary>
public class CreateConversationCommand : IRequest<ConversationDto>
{
    /// <summary>UUID định dạng dạng chuỗi của Khách hàng (User).</summary>
    public Guid UserId { get; set; }

    /// <summary>UUID định dạng dạng chuỗi của Thợ xem Tarot (Tarot Reader).</summary>
    public Guid ReaderId { get; set; }
}
