using MediatR;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Application.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

/// <summary>
/// Handler gộp toàn bộ Metadata Dashboard vào một cuộc gọi duy nhất.
/// TỐI ƯU HÓA Phase 4: Gộp thêm Notification List để triệt tiêu request storm.
/// </summary>
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

        /*
         * CHÚ Ý: DbContext của Entity Framework (SQL) không hỗ trợ truy cập song song 
         * trên cùng một instance Scoped. 
         * Vì vậy, Wallet và Streak (dùng DbContext) PHẢI chạy tuần tự (Sử dụng await trực tiếp).
         */
        
        // 1. Chạy tuần tự các tác vụ dùng DbContext (SQL)
        var wallet = await _mediator.Send(new GetWalletBalanceQuery(userId), cancellationToken);
        var streak = await _mediator.Send(new GetStreakStatusQuery { UserId = userId }, cancellationToken);

        // 2. Chạy song song các tác vụ dùng NoSQL/MongoDB (An toàn với đa luồng)
        var unreadNotificationTask = _notificationRepository.CountUnreadAsync(userId, cancellationToken);
        var unreadChatTask = _conversationRepository.GetTotalUnreadCountAsync(userIdString, cancellationToken);
        
        // Lấy 10 thông báo gần nhất cho Navbar Dropdown
        var recentNotificationsTask = _mediator.Send(new GetNotificationsQuery 
        { 
            UserId = userId, 
            Page = 1, 
            PageSize = 10 
        }, cancellationToken);

        // Lấy 100 cuộc hội thoại active cho Chat Sidebar
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

        // 3. Trả về DTO tổng hợp
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
