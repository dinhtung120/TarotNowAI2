/*
 * ===================================================================
 * FILE: ListMessagesQuery.cs
 * NAMESPACE: TarotNow.Application.Features.Chat.Queries.ListMessages
 * ===================================================================
 * MỤC ĐÍCH:
 *   Gói lệnh truy xuất Lịch Sử Tin Nhắn bên trong một Box Chat cụ thể.
 *   (Dùng khi bấm vào 1 dòng chat trên màn hình Inbox để mở giao diện nhắn tin).
 *
 * CHIẾN LƯỢC UI:
 *   Backend luôn trả về dữ liệu sắp xếp theo Thời gian Mới Nhất (DESC) 
 *   để phân trang. Khi đổ lên Frontend, điện thoại sẽ tự Reverse Lại 
 *   rồi ép cuộn xuống đáy giống iMessage/Zalo.
 * ===================================================================
 */

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

/// <summary>
/// Yêu cầu chiết xuất lịch sử hội thoại cho mục đích hiển thị Màn Hình Chat.
/// </summary>
public class ListMessagesQuery : IRequest<ListMessagesResult>
{
    /// <summary>Khoá chính của Cuộc trò chuyện cần lục lịch sử.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>Ai đang yêu cầu xem lịch sử này? (Dùng làm lá chắn bảo mật tránh nhìn lén).</summary>
    public Guid RequesterId { get; set; }

    public int Page { get; set; } = 1;
    
    // Mỗi lần vuốt Load More sẽ kéo 50 tin nhắn. 
    // Tránh bị đơ RAM Điện Thoại nếu có 10,000 tin nhắn.
    public int PageSize { get; set; } = 50;
}

public class ListMessagesResult
{
    public IEnumerable<ChatMessageDto> Messages { get; set; } = Enumerable.Empty<ChatMessageDto>();
    public long TotalCount { get; set; }
    
    // [YÊU CẦU MỚI]: Đính kèm thông tin Header cho Room Chat
    public ConversationDto Conversation { get; set; } = null!;
}

/// <summary>
/// Cổng kiểm duyệt An Ninh và kéo dữ liệu mảng Lịch sử chat từ hệ thống NoSQL.
/// </summary>
public class ListMessagesQueryHandler : IRequestHandler<ListMessagesQuery, ListMessagesResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;

    public ListMessagesQueryHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IUserRepository userRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
    }

    public async Task<ListMessagesResult> Handle(ListMessagesQuery request, CancellationToken cancellationToken)
    {
        // 1. Dò tìm Hộp thoại. Lỡ xoá mất rồi thì báo lỗi 404 (NotFound).
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // 2. NGĂN CHẶN XEM TRỘM GỐC RỄ:
        // Đứa xin danh sách tin nhắn này CÓ TUỔI GÌ MÀ ĐÒI XEM? CÓ TRONG HỘI THOẠI KHÔNG?
        // Nếu User C gọi API điền ID box chat của User A và Reader B -> Bị Đá Văng Ngay Lập Tức (BadRequest).
        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 3. Sau khi an toàn, lấy dữ liệu Mảng Tin Nhắn theo Trang (Phân trang Pagination).
        var (items, totalCount) = await _messageRepo.GetByConversationIdPaginatedAsync(
            request.ConversationId, request.Page, request.PageSize, cancellationToken);

        // 4. [YÊU CẦU MỚI]: Đính kèm Full-Name của Cả 2 User Trả Về Màn Hình Để Không Phải Fetch 2 Lần.
        var uniqueIds = new HashSet<Guid>();
        if (Guid.TryParse(conversation.UserId, out var uid)) uniqueIds.Add(uid);
        if (Guid.TryParse(conversation.ReaderId, out var rid)) uniqueIds.Add(rid);

        if (uniqueIds.Count > 0)
        {
            var userMap = await _userRepo.GetUserBasicInfoMapAsync(uniqueIds, cancellationToken);
            if (uid != Guid.Empty && userMap.TryGetValue(uid, out var userInfo))
            {
                conversation.UserName = userInfo.DisplayName;
                conversation.UserAvatar = userInfo.AvatarUrl;
            }
            if (rid != Guid.Empty && userMap.TryGetValue(rid, out var readerInfo))
            {
                conversation.ReaderName = readerInfo.DisplayName;
                conversation.ReaderAvatar = readerInfo.AvatarUrl;
            }
        }

        return new ListMessagesResult
        {
            Messages = items,
            TotalCount = totalCount,
            Conversation = conversation
        };
    }
}
