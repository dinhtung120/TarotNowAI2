using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Features.Chat.Queries.ListConversations;
using TarotNow.Application.Features.CheckIn.Queries.GetStreakStatus;
using TarotNow.Application.Features.Notification.Queries.GetNotifications;
using TarotNow.Application.Features.Wallet.Queries.GetWalletBalance;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.UserContext.Queries.GetInitialMetadata;

// Handler tổng hợp metadata đầu phiên cho user.
public class GetInitialMetadataQueryHandler : IRequestHandler<GetInitialMetadataQuery, UserMetadataDto>
{
    // Trang mặc định lấy notifications gần đây.
    private const int NotificationsPage = 1;

    // Số notifications gần đây trả về.
    private const int NotificationsPageSize = 10;

    // Trang mặc định lấy conversations active.
    private const int ActiveConversationsPage = 1;

    // Số conversations active trả về.
    private const int ActiveConversationsPageSize = 100;

    // Tab hội thoại cần lấy trong metadata khởi tạo.
    private const string ActiveConversationTab = "active";

    private readonly IMediator _mediator;
    private readonly INotificationRepository _notificationRepository;
    private readonly IConversationRepository _conversationRepository;

    /// <summary>
    /// Khởi tạo handler lấy initial metadata.
    /// Luồng xử lý: nhận mediator và repository trực tiếp để vừa gọi query hiện có vừa truy vấn count nhanh.
    /// </summary>
    public GetInitialMetadataQueryHandler(
        IMediator mediator,
        INotificationRepository notificationRepository,
        IConversationRepository conversationRepository)
    {
        _mediator = mediator;
        _notificationRepository = notificationRepository;
        _conversationRepository = conversationRepository;
    }

    /// <summary>
    /// Xử lý query lấy metadata khởi tạo.
    /// Luồng xử lý: lấy wallet + streak tuần tự, sau đó chạy song song các tác vụ unread/recent/active để giảm độ trễ response.
    /// </summary>
    public async Task<UserMetadataDto> Handle(GetInitialMetadataQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        var userIdString = userId.ToString();

        var wallet = await _mediator.Send(new GetWalletBalanceQuery(userId), cancellationToken);
        var streak = await _mediator.Send(new GetStreakStatusQuery { UserId = userId }, cancellationToken);
        // Lấy dữ liệu nền tảng trước để đảm bảo luôn có wallet/streak kể cả khi các tác vụ song song phía dưới chậm.

        var metadataTasks = BuildMetadataTasks(userId, userIdString, cancellationToken);
        await Task.WhenAll(
            metadataTasks.UnreadNotificationTask,
            metadataTasks.UnreadChatTask,
            metadataTasks.RecentNotificationsTask,
            metadataTasks.ActiveConversationsTask);
        // Chạy song song 4 tác vụ độc lập để tối ưu thời gian tổng hợp metadata.

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

    /// <summary>
    /// Dựng nhóm tác vụ lấy metadata có thể chạy song song.
    /// Luồng xử lý: tạo task đếm unread notification/chat, lấy notifications gần đây và lấy danh sách conversations active.
    /// </summary>
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

    // Nhóm task metadata dùng để gom kết quả song song một cách có kiểu dữ liệu rõ ràng.
    private readonly record struct MetadataTasks(
        Task<long> UnreadNotificationTask,
        Task<int> UnreadChatTask,
        Task<NotificationListResponse> RecentNotificationsTask,
        Task<ListConversationsResult> ActiveConversationsTask);
}
