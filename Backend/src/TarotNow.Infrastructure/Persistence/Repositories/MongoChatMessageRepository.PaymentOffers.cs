using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;
using TarotNow.Application.Common;
using TarotNow.Domain.Enums;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial kiểm tra phản hồi cho payment offer trong chat.
public partial class MongoChatMessageRepository
{
    /// <summary>
    /// Kiểm tra một payment offer đã có phản hồi accept/reject hay chưa.
    /// Luồng xử lý: lấy các message phản hồi gần đây trong conversation rồi parse content JSON để đối chiếu offerMessageId.
    /// </summary>
    public async Task<bool> HasPaymentOfferResponseAsync(
        string conversationId,
        string offerMessageId,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(offerMessageId))
        {
            return false;
            // Edge case: thiếu offer id thì không thể xác định phản hồi hợp lệ.
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
        // Giới hạn cửa sổ 100 message phản hồi để cân bằng đúng/sai và hiệu năng.

        return candidates.Any(item => TryReadOfferMessageId(item.Content, out var referenceId)
                                      && string.Equals(referenceId, offerMessageId, StringComparison.Ordinal));
        // So sánh Ordinal để tránh mismatch do culture và giữ id matching tuyệt đối.
    }

    /// <summary>
    /// Trích xuất offerMessageId từ content JSON của message phản hồi.
    /// Luồng xử lý: parse JSON, đọc property offerMessageId và validate chuỗi không rỗng.
    /// </summary>
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
                // Thiếu khóa offerMessageId thì message không được xem là phản hồi offer.
            }

            offerMessageId = offerIdNode.GetString() ?? string.Empty;
            return string.IsNullOrWhiteSpace(offerMessageId) == false;
        }
        catch
        {
            return false;
            // Edge case: content không phải JSON hợp lệ, bỏ qua thay vì ném exception phá luồng quét.
        }
    }
}
