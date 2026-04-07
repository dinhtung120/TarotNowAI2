

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface INotificationRepository
{
        Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default);

        Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default);

        Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default);

        Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default);

        Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default);
}

public class NotificationCreateDto
{
    public Guid UserId { get; set; }
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string TitleZh { get; set; } = string.Empty;
    public string BodyVi { get; set; } = string.Empty;
    public string BodyEn { get; set; } = string.Empty;
    public string BodyZh { get; set; } = string.Empty;
    public string Type { get; set; } = "system"; 
    public Dictionary<string, string>? Metadata { get; set; }
}

public class NotificationDto
{
    public string Id { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public string TitleVi { get; set; } = string.Empty;
    public string TitleEn { get; set; } = string.Empty;
    public string BodyVi { get; set; } = string.Empty;
    public string BodyEn { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}
