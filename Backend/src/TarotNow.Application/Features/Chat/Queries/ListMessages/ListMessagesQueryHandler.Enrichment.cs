using TarotNow.Application.Common;
using TarotNow.Domain.Enums;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public partial class ListMessagesQueryHandler
{
    private async Task EnrichParticipantProfilesAsync(
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        var userIds = new HashSet<Guid>();
        if (Guid.TryParse(conversation.UserId, out var userId))
        {
            userIds.Add(userId);
        }

        if (Guid.TryParse(conversation.ReaderId, out var readerId))
        {
            userIds.Add(readerId);
        }

        if (userIds.Count == 0)
        {
            return;
        }

        var userMap = await _userRepo.GetUserBasicInfoMapAsync(userIds, cancellationToken);
        if (userMap.TryGetValue(userId, out var userInfo))
        {
            conversation.UserName = userInfo.DisplayName;
            conversation.UserAvatar = userInfo.AvatarUrl;
        }

        if (userMap.TryGetValue(readerId, out var readerInfo))
        {
            conversation.ReaderName = readerInfo.DisplayName;
            conversation.ReaderAvatar = readerInfo.AvatarUrl;
        }
    }

    private async Task EnrichReaderStatusAndEscrowAsync(
        ConversationDto conversation,
        CancellationToken cancellationToken)
    {
        var readerProfile = await _readerProfileRepository.GetByUserIdAsync(conversation.ReaderId, cancellationToken);
        if (readerProfile != null)
        {
            var status = readerProfile.Status;
            if (_presenceTracker.IsOnline(readerProfile.UserId))
            {
                if (string.Equals(status, ReaderOnlineStatus.Offline, StringComparison.OrdinalIgnoreCase))
                {
                    status = ReaderOnlineStatus.Online;
                }
            }
            conversation.ReaderStatus = status;
        }

        var session = await _financeRepository.GetSessionByConversationRefAsync(conversation.Id, cancellationToken);
        if (session != null)
        {
            conversation.EscrowTotalFrozen = session.TotalFrozen;
            conversation.EscrowStatus = session.Status;
        }
    }
}
