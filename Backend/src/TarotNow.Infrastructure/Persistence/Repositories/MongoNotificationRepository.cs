

using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public class MongoNotificationRepository : INotificationRepository
{
    private readonly MongoDbContext _mongoContext;

    public MongoNotificationRepository(MongoDbContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

        public async Task CreateAsync(NotificationCreateDto notification, CancellationToken cancellationToken = default)
    {
        var doc = new NotificationDocument
        {
            UserId = notification.UserId.ToString(),
            Title = new LocalizedText
            {
                Vi = notification.TitleVi,
                En = notification.TitleEn,
                Zh = notification.TitleZh
            },
            Body = new LocalizedText
            {
                Vi = notification.BodyVi,
                En = notification.BodyEn,
                Zh = notification.BodyZh
            },
            Type = notification.Type,
            IsRead = false, 
            CreatedAt = DateTime.UtcNow
        };

        
        
        if (notification.Metadata != null && notification.Metadata.Count > 0)
        {
            doc.Metadata = new BsonDocument(notification.Metadata
                .Select(kv => new BsonElement(kv.Key, kv.Value)));
        }

        await _mongoContext.Notifications.InsertOneAsync(doc, cancellationToken: cancellationToken);
    }

        public async Task<(IEnumerable<NotificationDto> Items, long TotalCount)> GetByUserIdAsync(
        Guid userId, bool? isRead, int page, int pageSize, CancellationToken cancellationToken = default)
    {
        var normalizedPage = page < 1 ? 1 : page;
        var normalizedPageSize = pageSize <= 0 ? 20 : Math.Min(pageSize, 200);

        var userIdStr = userId.ToString();
        var filterBuilder = Builders<NotificationDocument>.Filter;

        
        var filter = filterBuilder.Eq(n => n.UserId, userIdStr);
        
        if (isRead.HasValue)
        {
            filter &= filterBuilder.Eq(n => n.IsRead, isRead.Value);
        }

        var totalCount = await _mongoContext.Notifications.CountDocumentsAsync(filter, cancellationToken: cancellationToken);

        var docs = await _mongoContext.Notifications
            .Find(filter)
            .SortByDescending(n => n.CreatedAt)
            .Skip((normalizedPage - 1) * normalizedPageSize)
            .Limit(normalizedPageSize)
            .ToListAsync(cancellationToken);

        
        var dtos = docs.Select(d => new NotificationDto
        {
            Id = d.Id,
            UserId = userId,
            TitleVi = d.Title.Vi,
            TitleEn = d.Title.En,
            BodyVi = d.Body.Vi,
            BodyEn = d.Body.En,
            Type = d.Type,
            IsRead = d.IsRead,
            CreatedAt = d.CreatedAt
        });

        return (dtos, totalCount);
    }

        public async Task<bool> MarkAsReadAsync(string notificationId, Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var result = await _mongoContext.Notifications.UpdateOneAsync(
            n => n.Id == notificationId && n.UserId == userIdStr, 
            Builders<NotificationDocument>.Update.Set(n => n.IsRead, true),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

        public async Task<bool> MarkAllAsReadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        var result = await _mongoContext.Notifications.UpdateManyAsync(
            n => n.UserId == userIdStr && !n.IsRead,
            Builders<NotificationDocument>.Update.Set(n => n.IsRead, true),
            cancellationToken: cancellationToken);

        return result.ModifiedCount > 0;
    }

        public async Task<long> CountUnreadAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var userIdStr = userId.ToString();

        return await _mongoContext.Notifications.CountDocumentsAsync(
            n => n.UserId == userIdStr && !n.IsRead,
            cancellationToken: cancellationToken);
    }
}
