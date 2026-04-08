using MongoDB.Bson;
using MongoDB.Driver;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial truy vấn payment offer đang pending/hết hạn.
public partial class MongoChatMessageRepository
{
    /// <summary>
    /// Tìm payment offer pending mới nhất trong hội thoại.
    /// Luồng xử lý: lấy một tập offer gần nhất, bỏ offer đã hết hạn hoặc đã có phản hồi, trả offer còn hiệu lực đầu tiên.
    /// </summary>
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
        // Giới hạn 20 để tối ưu latency, đủ bao phủ các offer gần đây của một conversation.

        foreach (var offer in offers)
        {
            if (offer.PaymentPayload?.ExpiresAt is DateTime expiresAt && expiresAt <= DateTime.UtcNow)
            {
                continue;
                // Offer hết hạn không còn giá trị nghiệp vụ nên bỏ qua.
            }

            var handled = await HasPaymentOfferResponseAsync(conversationId, offer.Id, cancellationToken);
            if (handled == false)
            {
                return ToDto(offer);
                // Trả ngay offer pending đầu tiên để caller xử lý nhanh.
            }
        }

        return null;
    }

    /// <summary>
    /// Lấy danh sách payment offer đã hết hạn nhưng chưa được xử lý.
    /// Luồng xử lý: lọc theo expires_at <= now, lấy theo thứ tự hết hạn sớm trước, rồi loại các offer đã có phản hồi.
    /// </summary>
    public async Task<IReadOnlyList<ChatMessageDto>> GetExpiredPendingPaymentOffersAsync(
        DateTime nowUtc,
        int limit = 200,
        CancellationToken cancellationToken = default)
    {
        var normalizedLimit = limit <= 0 ? 200 : Math.Min(limit, 1000);
        // Cho phép tăng limit cho batch job nhưng vẫn có trần cứng để bảo vệ tài nguyên.

        var filter = Builders<ChatMessageDocument>.Filter.And(
            Builders<ChatMessageDocument>.Filter.Eq(m => m.Type, ChatMessageType.PaymentOffer),
            Builders<ChatMessageDocument>.Filter.Eq(m => m.IsDeleted, false),
            Builders<ChatMessageDocument>.Filter.Ne("payment_payload", BsonNull.Value),
            Builders<ChatMessageDocument>.Filter.Ne("payment_payload.expires_at", BsonNull.Value),
            Builders<ChatMessageDocument>.Filter.Lte("payment_payload.expires_at", nowUtc));
        // Filter BSON trực tiếp vì expires_at nằm trong nested payload optional.

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
                // Chỉ giữ offer thật sự chưa xử lý để job không tác động lại dữ liệu đã chốt.
            }
        }

        return pending;
    }
}
