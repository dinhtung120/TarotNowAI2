/*
 * ===================================================================
 * FILE: GetNotificationsQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Queries.GetNotifications
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý GetNotificationsQuery — lấy danh sách thông báo từ MongoDB
 *   thông qua INotificationRepository.
 *
 * LUỒNG XỬ LÝ:
 *   1. Nhận Query chứa UserId, page, pageSize, isRead filter.
 *   2. Gọi repository.GetByUserIdAsync() → truy vấn MongoDB collection "notifications".
 *   3. Repository trả về tuple (Items, TotalCount) đã phân trang + sắp xếp.
 *   4. Pack kết quả vào NotificationListResponse → trả về Controller → trả về FE.
 *
 * TẠI SAO KHÔNG DÙNG DBCONTEXT TRỰC TIẾP?
 *   - Notification lưu trong MongoDB (không phải PostgreSQL/EF Core).
 *   - Truy cập qua INotificationRepository (Dependency Inversion Principle).
 *   - Handler ở Application Layer không biết MongoDB tồn tại → clean architecture.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

/// <summary>
/// Handler lấy danh sách thông báo phân trang từ MongoDB.
/// </summary>
public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, NotificationListResponse>
{
    /// <summary>Repository thông báo — inject qua DI, implement bởi MongoNotificationRepository.</summary>
    private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý query: gọi repository lấy dữ liệu, đóng gói response trả về.
    /// Repository đã xử lý sẵn việc normalize page/pageSize, sort by CreatedAt DESC.
    /// </summary>
    public async Task<NotificationListResponse> Handle(
        GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        /* Gọi repository — repository thực hiện:
         * 1. Filter theo UserId (kiểm tra ownership)
         * 2. Filter theo isRead nếu có
         * 3. Sort mới nhất trước (CreatedAt DESC)
         * 4. Skip/Limit cho phân trang
         * 5. Đếm total count cho pagination metadata
         */
        var (items, totalCount) = await _notificationRepository.GetByUserIdAsync(
            request.UserId,
            request.IsRead,
            request.Page,
            request.PageSize,
            cancellationToken);

        /* Đóng gói response — FE dùng để render list + pagination controls */
        return new NotificationListResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
