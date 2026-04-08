using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

// Query lấy danh sách notification theo bộ lọc và phân trang.
public class GetNotificationsQuery : IRequest<NotificationListResponse>
{
    // Định danh user cần lấy danh sách thông báo.
    public Guid UserId { get; set; }

    // Bộ lọc trạng thái đọc; null nghĩa là lấy cả đã đọc và chưa đọc.
    public bool? IsRead { get; set; }

    // Trang hiện tại.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;
}

// DTO danh sách notification kèm metadata phân trang.
public class NotificationListResponse
{
    // Danh sách thông báo của trang hiện tại.
    public IEnumerable<NotificationDto> Items { get; set; } = Enumerable.Empty<NotificationDto>();

    // Tổng số bản ghi khớp bộ lọc.
    public long TotalCount { get; set; }

    // Trang hiện tại.
    public int Page { get; set; }

    // Kích thước trang.
    public int PageSize { get; set; }
}
