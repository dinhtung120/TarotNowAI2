using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoChatMessageRepository
{
    public async Task<bool> HasPaymentOfferResponseAsync(
        string conversationId,
        string offerMessageId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(offerMessageId))
        {
            return false;
        }

        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.ConversationId, conversationId),
            Builders<ChatMessageDocument>.Filter.In(
                m => m.Type,
                new[] { ChatMessageType.PaymentAccept, ChatMessageType.PaymentReject }),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false));

        var candidates = await _context.ChatMessages
            .Find(filter)
            .SortByDescending(m => m.Id)
            .Limit(100)
            .ToListAsync(cancellationToken);

        return candidates.Any(item => TryReadOfferMessageId(item.Content, out var referenceId)
                                      && string.Equals(referenceId, offerMessageId, StringComparison.Ordinal));
    }

    private static bool TryReadOfferMessageId(string? content, out string offerMessageId)
    {
        offerMessageId = string.Empty;
        if (string.IsNullOrWhiteSpace(content))
        {
            return false;
        }

        try
        {
            using var document = JsonDocument.Parse(content);
            if (!document.RootElement.TryGetProperty("offerMessageId", out var offerIdNode))
            {
                return false;
            }

            offerMessageId = offerIdNode.GetString() ?? string.Empty;
            return string.IsNullOrWhiteSpace(offerMessageId) == false;
        }
        catch
        {
            return false;
        }
    }
}
