using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

public partial class ListConversationsQueryHandler
{
    /// <summary>
    /// Enrich tên/avatar của participant cho danh sách conversation.
    /// Luồng xử lý: gom toàn bộ participant id hợp lệ, tải map user basic info theo lô, rồi gán profile cho từng conversation.
    /// </summary>
    private async Task EnrichParticipantProfilesAsync(
        IEnumerable<ConversationDto> conversations,
        CancellationToken cancellationToken)
    {
        var userIds = CollectParticipantIds(conversations);
        if (userIds.Count == 0)
        {
            // Không có participant id hợp lệ thì bỏ qua enrich profile.
            return;
        }

        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, cancellationToken);
        foreach (var conversation in conversations)
        {
            ApplyUserProfile(conversation, userMap);
        }
    }

    /// <summary>
    /// Thu thập tập participant Guid duy nhất từ danh sách conversation.
    /// Luồng xử lý: parse UserId/ReaderId từng conversation và thêm vào HashSet để loại trùng.
    /// </summary>
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

    /// <summary>
    /// Thêm user id vào tập nếu chuỗi đầu vào parse được Guid.
    /// Luồng xử lý: parse Guid và chỉ thêm khi hợp lệ để tránh nhiễu dữ liệu.
    /// </summary>
    private static void AddUserIfGuid(ISet<Guid> users, string? value)
    {
        if (Guid.TryParse(value, out var parsed))
        {
            users.Add(parsed);
        }
    }

    /// <summary>
    /// Gán thông tin profile từ user map vào conversation.
    /// Luồng xử lý: map riêng cho user và reader, chỉ gán khi tìm thấy dữ liệu trong map.
    /// </summary>
    private static void ApplyUserProfile(
        ConversationDto conversation,
        IReadOnlyDictionary<Guid, (string DisplayName, string? AvatarUrl, string? ActiveTitle)> userMap)
    {
        if (Guid.TryParse(conversation.UserId, out var userId) && userMap.TryGetValue(userId, out var userInfo))
        {
            // Cập nhật profile phía user phục vụ UI inbox.
            conversation.UserName = userInfo.DisplayName;
            conversation.UserAvatar = userInfo.AvatarUrl;
        }

        if (Guid.TryParse(conversation.ReaderId, out var readerId) && userMap.TryGetValue(readerId, out var readerInfo))
        {
            // Cập nhật profile phía reader phục vụ UI inbox.
            conversation.ReaderName = readerInfo.DisplayName;
            conversation.ReaderAvatar = readerInfo.AvatarUrl;
        }
    }
}
