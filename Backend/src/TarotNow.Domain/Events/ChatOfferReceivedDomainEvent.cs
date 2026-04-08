using System;

namespace TarotNow.Domain.Events;

/// <summary>
/// Domain event phát sinh khi User gửi nội dung tư vấn đầu tiên, kích hoạt trạng thái cần phê duyệt từ Reader.
/// </summary>
public sealed class ChatOfferReceivedDomainEvent : IDomainEvent
{
    // Định danh cuộc trò chuyện
    public string ConversationId { get; init; } = string.Empty;

    // Định danh người dùng tạo yêu cầu
    public Guid UserId { get; init; }

    // Định danh reader nhận yêu cầu
    public Guid ReaderId { get; init; }

    // Thời hạn tối đa để reader phản hồi (UTC)
    public DateTime OfferExpiresAtUtc { get; init; }

    // Thời điểm phát sinh sự kiện (UTC)
    public DateTime OccurredAtUtc { get; init; } = DateTime.UtcNow;
}
