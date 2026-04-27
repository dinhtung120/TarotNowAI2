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

        var unreadNotifCount = await _notificationRepository.CountUnreadAsync(userId, cancellationToken);
        var unreadChatCount = await _conversationRepository.GetTotalUnreadCountAsync(userIdString, cancellationToken);
        var streakInfo = await _mediator.Send(new GetStreakStatusQuery { UserId = userId }, cancellationToken);
        var dropdownPreview = await _mediator.Send(
            new GetNotificationsQuery
            {
                UserId = userId,
                Page = 1,
                PageSize = DropdownPageSize
            },
            cancellationToken);

        return new NavbarSnapshotDto
        {
            UnreadNotificationCount = unreadNotifCount,
            UnreadChatCount = unreadChatCount < 0 ? 0 : unreadChatCount,
            Streak = streakInfo,
            DropdownPreview = dropdownPreview
        };
    }
}
