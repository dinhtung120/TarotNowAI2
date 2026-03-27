using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository
{
    public async Task<ChatMessageDto?> FindLatestPendingPaymentOfferAsync(
        string conversationId,
        CancellationToken cancellationToken = default)
    {
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Type, ChatMessageType.PaymentOffer),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var offers = await _context.ChatMessages
            .Find(filter)
            .SortByDescending(m => m.Id)
            .Limit(20)
            .ToListAsync(cancellationToken);

        foreach (var offer in offers)
        {
            if (offer.PaymentPayload?.ExpiresAt is DateTime expiresAt && expiresAt <= DateTime.UtcNow)
            {
                continue;
            }

            var handled = await HasPaymentOfferResponseAsync(conversationId, offer.Id, cancellationToken);
            if (handled == false)
            {
                return ToDto(offer);
            }
        }

        return null;
    }

    public async Task<IReadOnlyList<ChatMessageDto>> GetExpiredPendingPaymentOffersAsync(
        DateTime nowUtc,
        int limit = 200,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);
        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Type, ChatMessageType.PaymentOffer),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false),
            Builders<ChatMessageDocument>.Filter.Ne("payment_payload", BsonNull.Value),
            Builders<ChatMessageDocument>.Filter.Ne("payment_payload.expires_at", BsonNull.Value),
            Builders<ChatMessageDocument>.Filter.Lte("payment_payload.expires_at", nowUtc));

        var offers = await _context.ChatMessages
            .Find(filter)
            .SortBy(m => m.PaymentPayload!.ExpiresAt)
            .Limit(normalizedLimit)
            .ToListAsync(cancellationToken);

        var pending = new List<ChatMessageDto>();
        foreach (var offer in offers)
        {
            var handled = await HasPaymentOfferResponseAsync(offer.ConversationId, offer.Id, cancellationToken);
            if (handled == false)
            {
                pending.Add(ToDto(offer));
            }
        }

        return pending;
    }
}
