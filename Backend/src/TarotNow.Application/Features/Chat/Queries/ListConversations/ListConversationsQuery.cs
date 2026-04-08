

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

// Query lấy danh sách conversation theo tab inbox.
public class ListConversationsQuery : IRequest<ListConversationsResult>
{
    // Định danh user yêu cầu danh sách hội thoại.
    public Guid UserId { get; set; }

    // Tab lọc inbox: active/pending/completed/all.
    public string Tab { get; set; } = "active";

    // Trang hiện tại của danh sách.
    public int Page { get; set; } = 1;

    // Kích thước trang.
    public int PageSize { get; set; } = 20;
}

// DTO kết quả danh sách conversation.
public class ListConversationsResult
{
    // Danh sách conversation sau khi enrich.
    public IEnumerable<ConversationDto> Conversations { get; set; } = Enumerable.Empty<ConversationDto>();

    // Tổng số conversation thỏa điều kiện lọc.
    public long TotalCount { get; set; }

    // User hiện tại ở dạng chuỗi để client tiện so sánh participant.
    public string CurrentUserId { get; set; } = string.Empty;
}

// Handler truy vấn inbox conversations và enrich dữ liệu hiển thị.
public partial class ListConversationsQueryHandler : IRequestHandler<ListConversationsQuery, ListConversationsResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IUserRepository _userRepo;
    private readonly IChatFinanceRepository _financeRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IUserPresenceTracker _presenceTracker;

    /// <summary>
    /// Khởi tạo handler list conversations.
    /// Luồng xử lý: nhận repository conversation/user/finance/message và presence tracker để enrich dữ liệu inbox đầy đủ.
    /// </summary>
    public ListConversationsQueryHandler(
        IConversationRepository conversationRepo,
        IUserRepository userRepo,
        IChatFinanceRepository financeRepo,
        IReaderProfileRepository readerProfileRepo,
        IChatMessageRepository messageRepo,
        IUserPresenceTracker presenceTracker)
    {
        _conversationRepo = conversationRepo;
        _userRepo = userRepo;
        _financeRepo = financeRepo;
        _readerProfileRepo = readerProfileRepo;
        _messageRepo = messageRepo;
        _presenceTracker = presenceTracker;
    }

    /// <summary>
    /// Xử lý query lấy danh sách conversation.
    /// Luồng xử lý: tải danh sách theo tab + phân trang, enrich profile/trạng thái escrow/preview message, rồi trả kết quả tổng hợp.
    /// </summary>
    public async Task<ListConversationsResult> Handle(ListConversationsQuery request, CancellationToken cancellationToken)
    {
        var userId = request.UserId.ToString();
        var (items, totalCount) = await LoadConversationsAsync(userId, request, cancellationToken);
        // Enrich tuần tự để mỗi bước dùng dữ liệu bước trước một cách rõ ràng và dễ theo dõi.
        await EnrichParticipantProfilesAsync(items, cancellationToken);
        await EnrichReaderStatusAsync(items, cancellationToken);
        await EnrichEscrowSummaryAsync(items, cancellationToken);
        await EnrichLastMessagePreviewAsync(items, cancellationToken);

        return new ListConversationsResult
        {
            Conversations = items,
            TotalCount = totalCount,
            CurrentUserId = userId
        };
    }
}
