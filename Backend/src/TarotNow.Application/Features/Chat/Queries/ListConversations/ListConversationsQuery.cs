/*
 * ===================================================================
 * FILE: ListConversationsQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Queries.ListConversations
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh truy vấn lấy Danh Sách Phòng Chat (Màn Hình Inbox/Tin Nhắn).
 *   Sử dụng chung cho cả Khách Hàng (User) và Thầy bói Tarot (Reader).
 *
 * TÍNH NĂNG VƯỢT TRỘI:
 *   - Phân Trang (Pagination) giúp app tải nhanh, cuộn mượt mà.
 *   - Sắp xếp (Sort) theo `last_message_at DESC`: Phòng nào vừa nhắn tin 
 *     sẽ được trồi lên đầu danh sách y như Zalo/Messenger.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

/// <summary>
/// Yêu cầu cung cấp dữ liệu danh sách Box Chat từ phía Frontend.
/// </summary>
public class ListConversationsQuery : IRequest<ListConversationsResult>
{
    /// <summary>Truyền từ Header Auth JWT Token: Bạn là ai?</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Chế độ truy vấn (Ngữ cảnh thao tác).
    /// Vì TarotNow cho phép đóng 2 vai, bạn có thể xem Inbox góc độ Khách ("user") 
    /// hoặc xem Inbox góc độ làm ăn của Thợ ("reader").
    /// </summary>
    public string Role { get; set; } = "user";

    // Phân trang
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ListConversationsResult
{
    public IEnumerable<ConversationDto> Conversations { get; set; } = Enumerable.Empty<ConversationDto>();
    public long TotalCount { get; set; }
    public string CurrentUserId { get; set; } = string.Empty;
}

/// <summary>
/// Cỗ máy lấy dữ liệu từ NoSQL MongoDB trả về Frontend.
/// </summary>
public class ListConversationsQueryHandler : IRequestHandler<ListConversationsQuery, ListConversationsResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IUserRepository _userRepo;
    private readonly IChatFinanceRepository _financeRepo;

    public ListConversationsQueryHandler(
        IConversationRepository conversationRepo, 
        IUserRepository userRepo,
        IChatFinanceRepository financeRepo)
    {
        _conversationRepo = conversationRepo;
        _userRepo = userRepo;
        _financeRepo = financeRepo;
    }

    public async Task<ListConversationsResult> Handle(ListConversationsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToString();

        // [CẬP NHẬT THEO YÊU CẦU]: Bỏ rẽ nhánh Role ("user" hoặc "reader"). 
        // Lấy tất cả các Hộp thoại mà Client là người tham gia (Participant).
        var result = await _conversationRepo.GetByParticipantIdPaginatedAsync(userId, request.Page, request.PageSize, cancellationToken);
        var items = result.Items.ToList(); // Ép vật lý (Materialize) ở memory vì result.Items là IEnumerable lười (Lazy Evaluation)

        // Chuẩn bị danh sách ID để kéo tên và ảnh đại diện 1 mẻ duy nhất (Tránh bị N+1 Query vào Database)
        var uniqueUserIds = new HashSet<Guid>();
        foreach (var conv in items)
        {
            if (Guid.TryParse(conv.UserId, out var uid)) uniqueUserIds.Add(uid);
            if (Guid.TryParse(conv.ReaderId, out var rid)) uniqueUserIds.Add(rid);
        }

        if (uniqueUserIds.Count > 0)
        {
            // Tra cứu Map thông tin người dùng từ Entity Framework
            var userMap = await _userRepo.GetUserBasicInfoMapAsync(uniqueUserIds, cancellationToken);

            foreach (var conv in items)
            {
                if (Guid.TryParse(conv.UserId, out var uid) && userMap.TryGetValue(uid, out var userInfo))
                {
                    conv.UserName = userInfo.DisplayName;
                    conv.UserAvatar = userInfo.AvatarUrl;
                }
                if (Guid.TryParse(conv.ReaderId, out var rid) && userMap.TryGetValue(rid, out var readerInfo))
                {
                    conv.ReaderName = readerInfo.DisplayName;
                    conv.ReaderAvatar = readerInfo.AvatarUrl;
                }
            }
        }

        // [TÍNH NĂNG MỚI]: Bơm thông tin Ví tiền / Ký quỹ vào danh sách hiển thị
        if (items.Count > 0)
        {
            var conversationRefs = items.Select(x => x.Id).ToList();
            var activeSessions = await _financeRepo.GetSessionsByConversationRefsAsync(conversationRefs, cancellationToken);
            
            // Map bằng Dictionary để truy xuất thời gian O(1)
            var sessionMap = activeSessions.ToDictionary(s => s.ConversationRef);

            foreach (var conv in items)
            {
                if (sessionMap.TryGetValue(conv.Id, out var session))
                {
                    conv.EscrowTotalFrozen = session.TotalFrozen;
                    conv.EscrowStatus = session.Status;
                }
            }
        }

        return new ListConversationsResult
        {
            Conversations = items,
            TotalCount = result.TotalCount,
            CurrentUserId = userId
        };
    }
}
