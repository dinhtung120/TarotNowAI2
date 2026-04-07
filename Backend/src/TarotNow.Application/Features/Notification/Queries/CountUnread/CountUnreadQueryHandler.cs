

using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Notification.Queries.CountUnread;

public class CountUnreadQueryHandler : IRequestHandler<CountUnreadQuery, long>
{
    private readonly INotificationRepository _notificationRepository;

    public CountUnreadQueryHandler(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

        public async Task<long> Handle(CountUnreadQuery request, CancellationToken cancellationToken)
    {
        return await _notificationRepository.CountUnreadAsync(request.UserId, cancellationToken);
    }
}
