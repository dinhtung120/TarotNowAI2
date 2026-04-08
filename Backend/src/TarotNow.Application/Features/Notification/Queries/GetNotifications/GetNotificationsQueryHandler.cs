using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

// Handler lấy danh sách notification theo user.
public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, NotificationListResponse>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler truy vấn danh sách notification.
    /// Luồng xử lý: nhận notification repository để tải dữ liệu theo bộ lọc và phân trang.
    /// </summary>
    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý query lấy notification.
    /// Luồng xử lý: gọi repository theo UserId + IsRead + Page/PageSize và dựng response chuẩn cho client.
    /// </summary>
    public async Task<NotificationListResponse> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await _notificationRepository.GetByUserIdAsync(
            request.UserId,
            request.IsRead,
            request.Page,
            request.PageSize,
            cancellationToken);
        // Truy vấn một lần để lấy cả dữ liệu trang hiện tại và tổng bản ghi khớp bộ lọc.

        return new NotificationListResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
