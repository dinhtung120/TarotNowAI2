namespace TarotNow.Domain.Events;

// Domain event yêu cầu đồng bộ payment-accept message sau khi freeze add-money đã ghi bền vững.
public sealed class ConversationAddMoneyAcceptedSyncRequestedDomainEvent : IIdempotentDomainEvent
{
    private const string EventKeyPrefix = "chat:add-money-accept";

    // Định danh conversation chứa payment offer.
    public string ConversationId { get; }

    // Định danh user gửi phản hồi accept.
    public string SenderUserId { get; }

    // Định danh offer message gốc.
    public string OfferMessageId { get; }

    // Proposal id để truy vết nghiệp vụ; có thể rỗng với dữ liệu cũ.
    public string? ProposalId { get; }

    // Định danh message accept cần materialize trên Mongo.
    public string ResponseMessageId { get; }

    // Mốc phát sinh event theo UTC.
    public DateTime OccurredAtUtc { get; }

    // Khóa idempotency business-level để dedupe khi outbox retry.
    public string EventIdempotencyKey => $"{EventKeyPrefix}:{ConversationId}:{OfferMessageId}";

    /// <summary>
    /// Khởi tạo event đồng bộ phản hồi add-money accept.
    /// Luồng xử lý: chụp toàn bộ context cần cho handler materialize message idempotent.
    /// </summary>
    public ConversationAddMoneyAcceptedSyncRequestedDomainEvent(
        string conversationId,
        string senderUserId,
        string offerMessageId,
        string? proposalId,
        string responseMessageId,
        DateTime occurredAtUtc)
    {
        ConversationId = conversationId;
        SenderUserId = senderUserId;
        OfferMessageId = offerMessageId;
        ProposalId = proposalId;
        ResponseMessageId = responseMessageId;
        OccurredAtUtc = occurredAtUtc;
    }
}
