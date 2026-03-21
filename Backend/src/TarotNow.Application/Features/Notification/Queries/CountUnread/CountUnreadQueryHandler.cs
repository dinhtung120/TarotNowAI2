/*
 * ===================================================================
 * FILE: CountUnreadQueryHandler.cs
 * NAMESPACE: TarotNow.Application.Features.Notification.Queries.CountUnread
 * ===================================================================
 * MỤC ĐÍCH:
 *   Handler đếm thông báo chưa đọc qua INotificationRepository.CountUnreadAsync().
 *   Trả về 1 con số duy nhất — nhẹ, nhanh, phù hợp polling từ FE.
 *
 * PERFORMANCE:
 *   MongoDB CountDocuments với filter (userId + IsRead=false) rất nhanh
 *   vì đã có compound index trên collection "notifications".
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

/// <summary>
/// Handler đếm thông báo chưa đọc — gọi repository và trả số nguyên.
/// </summary>
public class CountUnreadQueryHandler : IRequestHandler<CountUnreadQuery, long>
{
    private readonly INotificationRepository _notificationRepository;

    public CountUnreadQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý query: gọi repository đếm thông báo IsRead=false của user.
    /// Kết quả trả thẳng — không cần mapping hay transform gì thêm.
    /// </summary>
    public async Task<long> Handle(CountUnreadQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.CountUnreadAsync(request.UserId, cancellationToken);
    }
}
