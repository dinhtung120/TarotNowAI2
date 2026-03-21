/*
 * ===================================================================
 * FILE: MarkNotificationReadCommand.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Commands.MarkAsRead
 * ===================================================================
 * MỤC ĐÍCH:
 *   CQRS Command đánh dấu 1 thông báo đã đọc.
 *   Đây là thao tác GHI (mutate state) → dùng Command (không phải Query).
 *
 * BẢO MẬT:
 *   - UserId được controller gán từ JWT claim (không để client tự truyền).
 *   - Repository sẽ kiểm tra OWNERSHIP: chỉ mark được thông báo CỦA MÌNH.
 *   - Ngăn chặn IDOR attack (user A mark thông báo của user B).
 * ===================================================================
 */

using MediatR;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

/// <summary>
/// Command đánh dấu 1 thông báo đã đọc. Trả về bool: true=thành công, false=không tìm thấy.
/// </summary>
public class MarkNotificationReadCommand : IRequest<bool>
{
    /// <summary>ID thông báo cần đánh dấu (MongoDB ObjectId dạng string).</summary>
    public string NotificationId { get; set; } = string.Empty;

    /// <summary>
    /// ID user — controller gán từ JWT claim.
    /// Dùng kiểm tra ownership: user chỉ mark được notification của mình.
    /// </summary>
    public Guid UserId { get; set; }
}
