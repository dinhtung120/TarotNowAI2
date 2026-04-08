using MediatR;
using System;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

// Query đếm số lượng thông báo chưa đọc của user.
public class CountUnreadQuery : IRequest<long>
{
    // Định danh user cần thống kê thông báo chưa đọc.
    public Guid UserId { get; set; }

    /// <summary>
    /// Khởi tạo query đếm unread theo user id.
    /// Luồng xử lý: gán UserId ngay tại thời điểm tạo query để giữ ngữ cảnh truy vấn nhất quán.
    /// </summary>
    public CountUnreadQuery(Guid userId)
    {
        UserId = userId;
    }
}
