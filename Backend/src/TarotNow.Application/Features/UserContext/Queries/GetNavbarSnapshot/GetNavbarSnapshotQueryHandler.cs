using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.UserContext.Queries.GetNavbarSnapshot;

public class GetNavbarSnapshotQueryHandler : IRequestHandler<GetNavbarSnapshotQuery, NavbarSnapshotDto>
{
    private const int DropdownPageSize = 10;

    private readonly IMediator _mediator;
    private readonly INotificationRepository _notificationRepository;
    private readonly IConversationRepository _conversationRepository;

    public GetNavbarSnapshotQueryHandler(
        IMediator mediator,
        INotificationRepository notificationRepository,
        IConversationRepository conversationRepository)
    {
        _mediator = mediator;
        _notificationRepository = notificationRepository;
        _conversationRepository = conversationRepository;
    }

    public async Task<NavbarSnapshotDto> Handle(GetNavbarSnapshotQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var userIdString = userId.ToString();

        var unreadNotifTask = _notificationRepository.CountUnreadAsync(userId, cancellationToken);
        var unreadChatTask = _conversationRepository.GetTotalUnreadCountAsync(userIdString, cancellationToken);
        var streakTask = _mediator.Send(new GetStreakStatusQuery { UserId = userId }, cancellationToken);
        var dropdownTask = _mediator.Send(
            new GetNotificationsQuery
            {
                UserId = userId,
                Page = 1,
                PageSize = DropdownPageSize
            },
            cancellationToken);

        await Task.WhenAll(unreadNotifTask, unreadChatTask, streakTask, dropdownTask);

        return new NavbarSnapshotDto
        {
            UnreadNotificationCount = (int)await unreadNotifTask,
            UnreadChatCount = await unreadChatTask,
            Streak = await streakTask,
            DropdownPreview = await dropdownTask
        };
    }
}
