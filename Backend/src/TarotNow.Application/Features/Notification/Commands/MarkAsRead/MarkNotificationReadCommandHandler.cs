/*
 * ===================================================================
 * FILE: MarkNotificationReadCommandHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Commands.MarkAsRead
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler xử lý MarkNotificationReadCommand — đánh dấu thông báo đã đọc.
 *
 * LUỒNG XỬ LÝ:
 *   1. Nhận Command chứa NotificationId + UserId.
 *   2. Gọi repository.MarkAsReadAsync() → cập nhật IsRead=true trong MongoDB.
 *   3. Repository kiểm tra OWNERSHIP (userId match) trước khi update.
 *   4. Trả về true nếu cập nhật thành công, false nếu không tìm thấy.
 *
 * TẠI SAO TRẢ BOOL THAY VÌ THROW EXCEPTION?
 *   - Notification "không tìm thấy" không phải lỗi nghiêm trọng.
 *   - Có thể do notification đã bị TTL xóa (quá 30 ngày).
 *   - Controller sẽ return NotFound nếu false → FE hiển thị thông báo phù hợp.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Commands.MarkAsRead;

/// <summary>
/// Handler đánh dấu thông báo đã đọc — delegate cho repository với ownership check.
/// </summary>
public class MarkNotificationReadCommandHandler : IRequestHandler<MarkNotificationReadCommand, bool>
{
    private readonly INotificationRepository _notificationRepository;

    public MarkNotificationReadCommandHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý command: gọi repository mark as read.
    /// Repository đã bao gồm filter userId → đảm bảo user chỉ mark thông báo của mình.
    /// </summary>
    public async Task<bool> Handle(MarkNotificationReadCommand request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.MarkAsReadAsync(
            request.NotificationId,
            request.UserId,
            cancellationToken);
    }
}
