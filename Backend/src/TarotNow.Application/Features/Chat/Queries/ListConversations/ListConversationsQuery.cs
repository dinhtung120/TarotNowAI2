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
}

/// <summary>
/// Cỗ máy lấy dữ liệu từ NoSQL MongoDB trả về Frontend.
/// </summary>
public class ListConversationsQueryHandler : IRequestHandler<ListConversationsQuery, ListConversationsResult>
{
    private readonly IConversationRepository _conversationRepo;

    public ListConversationsQueryHandler(IConversationRepository conversationRepo)
    {
        _conversationRepo = conversationRepo;
    }

    public async Task<ListConversationsResult> Handle(ListConversationsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToString();
        (IEnumerable<ConversationDto> items, long totalCount) result;

        // Cơ chế Rẽ Nhánh Truy Vấn (Query Dispatch):
        // Nếu bạn là Reader -> Trích xuất danh sách khách hàng đang chờ/đang chat với bạn.
        // Nếu bạn là User -> Trích xuất danh sách các Thầy Tarot bạn đã book.
        if (request.Role == "reader")
            result = await _conversationRepo.GetByReaderIdPaginatedAsync(userId, request.Page, request.PageSize, cancellationToken);
        else
            result = await _conversationRepo.GetByUserIdPaginatedAsync(userId, request.Page, request.PageSize, cancellationToken);

        return new ListConversationsResult
        {
            Conversations = result.items,
            TotalCount = result.totalCount
        };
    }
}
