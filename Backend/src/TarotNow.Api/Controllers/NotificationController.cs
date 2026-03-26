/*
 * ===================================================================
 * FILE: NotificationController.cs
 * NAMESPACE: TarotNow.Api.Controllers
 * ===================================================================
 * MỤC ĐÍCH TỔNG QUAN:
 *   Controller quản lý THÔNG BÁO IN-APP (Notifications) của người dùng.
 *   Cho phép xem danh sách thông báo, đếm chưa đọc, và đánh dấu đã đọc.
 *
 * CƠ CHẾ THÔNG BÁO:
 *   - Thông báo được tạo bởi các sự kiện trong hệ thống (nạp tiền, đọc bài, escrow...)
 *   - Lưu trữ trong MongoDB collection "notifications" với TTL 30 ngày tự xóa.
 *   - Hỗ trợ đa ngôn ngữ (vi/en/zh) — FE chọn hiển thị theo locale user.
 *   - Badge count (số đỏ trên icon chuông) poll endpoint unread-count mỗi 30-60s.
 *
 * ENDPOINTS:
 *   - GET  /notifications           → Danh sách thông báo (phân trang, filter)
 *   - GET  /notifications/unread-count → Đếm thông báo chưa đọc
 *   - PATCH /notifications/{id}/read  → Đánh dấu 1 thông báo đã đọc
 *
 * BẢO MẬT:
 *   - [Authorize] toàn bộ controller → bắt buộc đăng nhập.
 *   - UserId lấy từ JWT claim, không cho client tự truyền → chống IDOR.
 * ===================================================================
 */

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

/* Import các CQRS Query/Command cho notification */
using TarotNow.Application.Features.Notification.Commands.MarkAsRead;
using TarotNow.Application.Features.Notification.Queries.CountUnread;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Api.Extensions;

namespace TarotNow.Api.Controllers;

/// <summary>
/// Controller xử lý thông báo in-app.
/// Tất cả endpoint yêu cầu user đã đăng nhập (JWT valid).
/// </summary>
[Route("api/v1/[controller]")]
[ApiController]
[Authorize]
public class NotificationController : ControllerBase
{
    /// <summary>MediatR mediator — route commands/queries đến đúng handler.</summary>
    private readonly IMediator _mediator;

    public NotificationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Notification?page=1&amp;pageSize=20&amp;isRead=false
    /// MỤC ĐÍCH: Lấy danh sách thông báo của user hiện tại.
    ///
    /// THAM SỐ (Query string):
    ///   - page: số trang (mặc định 1)
    ///   - pageSize: số item/trang (mặc định 20, tối đa 200)
    ///   - isRead: filter đọc/chưa đọc (null=tất cả, true=đã đọc, false=chưa đọc)
    ///
    /// TRẢ VỀ:
    ///   - items: danh sách NotificationDto (title/body đa ngôn ngữ, type, isRead, createdAt)
    ///   - totalCount: tổng số thông báo (để FE render pagination)
    ///   - page, pageSize: echo lại param cho FE
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(NotificationListResponse))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] bool? isRead = null)
    {
        /*
         * Lấy UserId từ JWT claim.
         * ClaimTypes.NameIdentifier chứa GUID user đã đăng nhập.
         * Nếu parse thất bại → JWT không hợp lệ → trả 401.
         */
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        /* Tạo Query object — controller KHÔNG chứa business logic,
         * chỉ map HTTP params → CQRS Query rồi gửi qua MediatR. */
        var query = new GetNotificationsQuery
        {
            UserId = userId,
            Page = page,
            PageSize = pageSize,
            IsRead = isRead
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    /// <summary>
    /// ENDPOINT: GET /api/v1/Notification/unread-count
    /// MỤC ĐÍCH: Đếm số thông báo chưa đọc — dùng cho badge count trên icon chuông.
    ///
    /// TRẢ VỀ: { "count": 5 }
    ///
    /// PERFORMANCE:
    ///   FE poll endpoint này mỗi 30-60s. Query nhẹ (chỉ count, không load data)
    ///   nên không ảnh hưởng performance dù poll thường xuyên.
    /// </summary>
    [HttpGet("unread-count")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> GetUnreadCount()
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        /* Query đơn giản: chỉ cần UserId, trả về 1 con số */
        var count = await _mediator.Send(new CountUnreadQuery(userId));

        /* Wrap trong object thay vì trả number trần
         * → FE dễ parse: result.count thay vì xử lý plain number
         * → Extensible: tương lai thêm lastCheckedAt, v.v. */
        return Ok(new { count });
    }

    /// <summary>
    /// ENDPOINT: PATCH /api/v1/Notification/{id}/read
    /// MỤC ĐÍCH: Đánh dấu 1 thông báo đã đọc.
    ///
    /// TẠI SAO DÙNG PATCH THAY VÌ PUT?
    ///   - PATCH: cập nhật MỘT PHẦN resource (chỉ thay đổi field isRead).
    ///   - PUT: thay thế TOÀN BỘ resource → không phù hợp ở đây.
    ///   - Theo chuẩn REST, PATCH chính xác hơn cho partial update.
    ///
    /// BẢO MẬT:
    ///   - UserId từ JWT → chỉ mark được notification CỦA MÌNH.
    ///   - Repository kiểm tra: notificationId + userId phải khớp.
    ///   - Nếu user A cố mark notification của user B → trả 404 (không tiết lộ tồn tại).
    /// </summary>
    [HttpPatch("{id}/read")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> MarkAsRead(string id)
    {
        if (!User.TryGetUserId(out var userId))
            return Unauthorized();

        /* Command chứa NotificationId + UserId cho ownership check */
        var command = new MarkNotificationReadCommand
        {
            NotificationId = id,
            UserId = userId
        };

        var success = await _mediator.Send(command);

        if (!success)
        {
            /* false = không tìm thấy notification hoặc không phải của user.
             * Trả 404 thay vì 403 → không tiết lộ notification có tồn tại hay không
             * → chống thông tin rò rỉ (information disclosure). */
            return NotFound(new { message = "Notification not found." });
        }

        return Ok(new { message = "Notification marked as read." });
    }
}
