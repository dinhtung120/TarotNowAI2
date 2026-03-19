/*
 * ===================================================================
 * FILE: SendMessageCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Commands.SendMessage
 * ===================================================================
 * MỤC ĐÍCH:
 *   Command kích hoạt luồng "Gửi Tin Nhắn Mới" vào Box Chat.
 *
 * PHÂN LOẠI TIN NHẮN:
 *   Nền tảng TarotNow không chỉ có chữ (Text message), mà còn hỗ trợ:
 *   - "payment_offer": Lời đề nghị thu phí (Reader yêu cầu User trả tiền để xem tiếp).
 *   - "image": Gửi ảnh chụp trải bài.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Commands.SendMessage;

/// <summary>
/// Gói dữ liệu chứa nội dung cần truyền tải.
/// Trả về DTO ChatMessage hoàn chỉnh mang theo dấu thời gian để bắn qua Socket (SignalR).
/// </summary>
public class SendMessageCommand : IRequest<ChatMessageDto>
{
    /// <summary>Vị trí nhận tin (ID Box Chat).</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>Người thực hiện gõ phím.</summary>
    public Guid SenderId { get; set; }

    /// <summary>Thể loại dữ liệu bắn đi (text, image, payment_offer).</summary>
    public string Type { get; set; } = "text";

    /// <summary>Nội dung dạng thô (nếu là chữ).</summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>Đặc quyền mở rộng: Chứa gói thông tin Báo Giá tiền (Chỉ gởi khi Type = payment_offer).</summary>
    public PaymentPayloadDto? PaymentPayload { get; set; }
}
