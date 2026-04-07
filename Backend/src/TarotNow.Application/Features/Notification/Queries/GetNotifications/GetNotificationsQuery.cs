

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.GetNotifications;

public class GetNotificationsQuery : IRequest<NotificationListResponse>
{
        public Guid UserId { get; set; }

        public bool? IsRead { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
}

public class NotificationListResponse
{
        public IEnumerable<NotificationDto> Items { get; set; } = Enumerable.Empty<NotificationDto>();

        public long TotalCount { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
}
