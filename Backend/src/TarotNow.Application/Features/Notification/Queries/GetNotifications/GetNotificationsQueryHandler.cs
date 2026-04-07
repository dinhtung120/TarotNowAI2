

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

public class GetNotificationsQueryHandler : IRequestHandler<GetNotificationsQuery, NotificationListResponse>
{
        private readonly INotificationRepository _notificationRepository;

    public GetNotificationsQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

        public async Task<NotificationListResponse> Handle(
        GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        
        var (items, totalCount) = await _notificationRepository.GetByUserIdAsync(
            request.UserId,
            request.IsRead,
            request.Page,
            request.PageSize,
            cancellationToken);

        
        return new NotificationListResponse
        {
            Items = items,
            TotalCount = totalCount,
            Page = request.Page,
            PageSize = request.PageSize
        };
    }
}
