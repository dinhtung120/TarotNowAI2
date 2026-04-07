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

        
        var unreadNotificationTask = _notificationRepository.CountUnreadAsync(userId, cancellationToken);
        var unreadChatTask = _conversationRepository.GetTotalUnreadCountAsync(userIdString, cancellationToken);
        
        
        var recentNotificationsTask = _mediator.Send(new GetNotificationsQuery 
        { 
            UserId = userId, 
            Page = 1, 
            PageSize = 10 
        }, cancellationToken);

        
        var activeConversationsTask = _mediator.Send(new ListConversationsQuery
        {
            UserId = userId,
            Tab = "active",
            Page = 1,
            PageSize = 100
        }, cancellationToken);

        await Task.WhenAll(
            unreadNotificationTask, 
            unreadChatTask, 
            recentNotificationsTask, 
            activeConversationsTask);

        
        return new UserMetadataDto
        {
            Wallet = wallet,
            Streak = streak,
            UnreadNotificationCount = (int)await unreadNotificationTask,
            RecentNotifications = await recentNotificationsTask,
            UnreadChatCount = await unreadChatTask,
            ActiveConversations = await activeConversationsTask
        };
    }
}
