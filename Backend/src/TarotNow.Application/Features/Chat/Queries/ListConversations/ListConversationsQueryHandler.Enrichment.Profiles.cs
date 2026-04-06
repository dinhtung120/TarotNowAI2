using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    private async Task EnrichParticipantProfilesAsync(
        IEnumerable<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        var userIds = CollectParticipantIds(conversations);
        if (userIds.Count == 0)
        {
            return;
        }

        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, cancellationToken);
        foreach (var conversation in conversations)
        {
            ApplyUserProfile(conversation, userMap);
        }
    }

    private static HashSet<Guid> CollectParticipantIds(IEnumerable<ConversationDto> conversations)
    {
        var userIds = new HashSet<Guid>();
        foreach (var conversation in conversations)
        {
            AddUserIfGuid(userIds, conversation.UserId);
            AddUserIfGuid(userIds, conversation.ReaderId);
        }

        return userIds;
    }

    private static void AddUserIfGuid(ISet<Guid> users, string? value)
    {
        if (Guid.TryParse(value, out var parsed))
        {
            users.Add(parsed);
        }
    }

    private static void ApplyUserProfile(
        ConversationDto conversation,
        IReadOnlyDictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)> userMap)
    {
        if (Guid.TryParse(conversation.UserId, out var userId) && userMap.TryGetValue(userId, out var userInfo))
        {
            conversation.UserName = userInfo.DisplayName;
            conversation.UserAvatar = userInfo.AvatarUrl;
        }

        if (Guid.TryParse(conversation.ReaderId, out var readerId) && userMap.TryGetValue(readerId, out var readerInfo))
        {
            conversation.ReaderName = readerInfo.DisplayName;
            conversation.ReaderAvatar = readerInfo.AvatarUrl;
        }
    }
}
