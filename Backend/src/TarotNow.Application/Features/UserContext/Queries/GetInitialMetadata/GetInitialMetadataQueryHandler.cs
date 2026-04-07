using MediatR;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

public class GetInitialMetadataQueryHandler : IRequestHandler<GetInitialMetadataQuery, UserMetadataDto>
{
    private const int NotificationsPage = 1;
    private const int NotificationsPageSize = 10;
    private const int ActiveConversationsPage = 1;
    private const int ActiveConversationsPageSize = 100;
    private const string ActiveConversationTab = "active";

    private readonly IMediator _mediator;
    private readonly INotificationRepository _notificationRepository;
    private readonly IConversationRepository _conversationRepository;

    public GetInitialMetadataQueryHandler(
        IMediator mediator,
        INotificationRepository notificationRepository,
        IConversationRepository conversationRepository)
    {
        _mediator = mediator;
        _notificationRepository = notificationRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<UserMetadataDto> Handle(GetInitialMetadataQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var userIdString = userId.ToString();

        var wallet = await _mediator.Send(new GetWalletBalanceQuery(userId), cancellationToken);
        var streak = await _mediator.Send(new GetStreakStatusQuery { UserId = userId }, cancellationToken);

        var metadataTasks = BuildMetadataTasks(userId, userIdString, cancellationToken);
        await Task.WhenAll(
            metadataTasks.UnreadNotificationTask,
            metadataTasks.UnreadChatTask,
            metadataTasks.RecentNotificationsTask,
            metadataTasks.ActiveConversationsTask);

        return new UserMetadataDto
        {
            Wallet = wallet,
            Streak = streak,
            UnreadNotificationCount = (int)await metadataTasks.UnreadNotificationTask,
            RecentNotifications = await metadataTasks.RecentNotificationsTask,
            UnreadChatCount = await metadataTasks.UnreadChatTask,
            ActiveConversations = await metadataTasks.ActiveConversationsTask
        };
    }

    private MetadataTasks BuildMetadataTasks(
        Guid userId,
        string userIdString,
        CancellationToken cancellationToken)
    {
        return new MetadataTasks(
            UnreadNotificationTask: _notificationRepository.CountUnreadAsync(userId, cancellationToken),
            UnreadChatTask: _conversationRepository.GetTotalUnreadCountAsync(userIdString, cancellationToken),
            RecentNotificationsTask: _mediator.Send(
                new GetNotificationsQuery
                {
                    UserId = userId,
                    Page = NotificationsPage,
                    PageSize = NotificationsPageSize
                },
                cancellationToken),
            ActiveConversationsTask: _mediator.Send(
                new ListConversationsQuery
                {
                    UserId = userId,
                    Tab = ActiveConversationTab,
                    Page = ActiveConversationsPage,
                    PageSize = ActiveConversationsPageSize
                },
                cancellationToken));
    }

    private readonly record struct MetadataTasks(
        Task<long> UnreadNotificationTask,
        Task<int> UnreadChatTask,
        Task<NotificationListResponse> RecentNotificationsTask,
        Task<ListConversationsResult> ActiveConversationsTask);
}
