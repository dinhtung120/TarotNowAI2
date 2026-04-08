using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

// Partial xử lý tính tổng unread theo participant.
public partial class MongoConversationRepository
{
    /// <summary>
    /// Tính tổng số message chưa đọc của participant trên mọi conversation.
    /// Luồng xử lý: lọc conversation participant tham gia, project unread tối thiểu rồi cộng theo vai trò user/reader.
    /// </summary>
    public async Task<int> GetTotalUnreadCountAsync(string participantId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Or(
                Builders<ConversationDocument>.Filter.Eq(c => c.UserId, participantId),
                Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, participantId)),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));
        // Bỏ qua conversation đã xóa mềm để không cộng unread lịch sử không còn hiển thị.

        var conversations = await _context.Conversations
            .Find(filter)
            .Project(c => new ParticipantUnreadProjection
            {
                UserId = c.UserId,
                UserUnread = c.UnreadCount.User,
                ReaderUnread = c.UnreadCount.Reader
            })
            .ToListAsync(cancellationToken);

        return conversations.Sum(c => c.UserId == participantId ? c.UserUnread : c.ReaderUnread);
        // Luồng tính tách vai trò giúp trả đúng unread của participant trong từng conversation.
    }

    // Projection nội bộ chỉ chứa dữ liệu cần cho phép tính unread.
    private sealed class ParticipantUnreadProjection
    {
        // UserId gốc của conversation để xác định participant đóng vai user hay reader.
        public string UserId { get; init; } = string.Empty;
        // Số unread phía user.
        public int UserUnread { get; init; }
        // Số unread phía reader.
        public int ReaderUnread { get; init; }
    }
}
