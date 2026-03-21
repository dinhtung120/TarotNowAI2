/*
 * ===================================================================
 * FILE: GetNotificationsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Queries.GetNotifications
 * ===================================================================
 * MỤC ĐÍCH:
 *   CQRS Query để lấy danh sách thông báo in-app của user hiện tại.
 *   Hỗ trợ phân trang (page/pageSize) và filter đọc/chưa đọc (isRead).
 *
 * TẠI SAO DÙNG CQRS (QUERY)?
 *   - Đây là thao tác ĐỌC dữ liệu, không thay đổi state → dùng Query (không phải Command).
 *   - MediatR sẽ tự động route Query này đến đúng Handler.
 *   - Tách biệt rõ ràng giữa đọc (Query) và ghi (Command) → clean architecture.
 *
 * RESPONSE TYPE:
 *   NotificationListResponse — chứa danh sách NotificationDto + TotalCount
 *   (không dùng PaginatedList<T> vì notification lưu MongoDB, không có EF queryable).
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

/// <summary>
/// Query lấy danh sách thông báo của user.
/// Controller gán UserId từ JWT claim trước khi gửi qua MediatR.
/// </summary>
public class GetNotificationsQuery : IRequest<NotificationListResponse>
{
    /// <summary>ID user lấy từ JWT — controller gán, không để client tự truyền.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Filter trạng thái đọc/chưa đọc:
    /// - null: tất cả thông báo
    /// - true: chỉ đã đọc
    /// - false: chỉ chưa đọc
    /// </summary>
    public bool? IsRead { get; set; }

    /// <summary>Số trang (1-indexed). Mặc định trang 1.</summary>
    public int Page { get; set; } = 1;

    /// <summary>Số thông báo mỗi trang. Mặc định 20, tối đa 200.</summary>
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// Response chứa danh sách thông báo + metadata phân trang.
/// FE dùng TotalCount để render pagination component.
/// </summary>
public class NotificationListResponse
{
    /// <summary>Danh sách thông báo trong trang hiện tại.</summary>
    public IEnumerable<NotificationDto> Items { get; set; } = Enumerable.Empty<NotificationDto>();

    /// <summary>Tổng số thông báo (tất cả trang, sau filter).</summary>
    public long TotalCount { get; set; }

    /// <summary>Trang hiện tại.</summary>
    public int Page { get; set; }

    /// <summary>Kích thước trang.</summary>
    public int PageSize { get; set; }
}
