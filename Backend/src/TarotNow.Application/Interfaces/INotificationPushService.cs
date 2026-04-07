

using System;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface INotificationPushService
{
        Task PushNewNotificationAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

        Task SendEventAsync(string userId, string eventName, object payload, CancellationToken cancellationToken = default);
}
