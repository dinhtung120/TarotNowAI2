using MongoDB.Driver;
using TarotNow.Infrastructure.Persistence.MongoDocuments;

namespace TarotNow.Infrastructure.Persistence.Repositories;

public partial class MongoConversationRepository
{
    public async Task<int> GetTotalUnreadCountAsync(string participantId, CancellationToken cancellationToken = default)
    {
        var filter = Builders<ConversationDocument>.Filter.And(
            Builders<ConversationDocument>.Filter.Or(
                Builders<ConversationDocument>.Filter.Eq(c => c.UserId, participantId),
                Builders<ConversationDocument>.Filter.Eq(c => c.ReaderId, participantId)),
            Builders<ConversationDocument>.Filter.Eq(c => c.IsDeleted, false));

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
    }

    private sealed class ParticipantUnreadProjection
    {
        public string UserId { get; init; } = string.Empty;
        public int UserUnread { get; init; }
        public int ReaderUnread { get; init; }
    }
}
