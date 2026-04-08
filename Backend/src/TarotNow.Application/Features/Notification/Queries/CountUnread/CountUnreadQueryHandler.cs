using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

// Handler truy vấn số lượng thông báo chưa đọc.
public class CountUnreadQueryHandler : IRequestHandler<CountUnreadQuery, long>
{
    private readonly INotificationRepository _notificationRepository;

    /// <summary>
    /// Khởi tạo handler đếm unread notification.
    /// Luồng xử lý: nhận notification repository để thực hiện truy vấn thống kê.
    /// </summary>
    public CountUnreadQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    /// <summary>
    /// Xử lý query đếm thông báo chưa đọc.
    /// Luồng xử lý: truy vấn repository theo UserId và trả số lượng long để UI hiển thị badge.
    /// </summary>
    public async Task<long> Handle(CountUnreadQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.CountUnreadAsync(request.UserId, cancellationToken);
    }
}
