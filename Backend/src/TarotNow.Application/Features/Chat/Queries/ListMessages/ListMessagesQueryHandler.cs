using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Common.Interfaces;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

// Handler lấy danh sách message theo cursor và enrich metadata conversation.
public partial class ListMessagesQueryHandler : IRequestHandler<ListMessagesQuery, ListMessagesResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IChatFinanceRepository _financeRepository;
    private readonly IUserPresenceTracker _presenceTracker;
    private readonly IConversationReviewRepository _conversationReviewRepository;

    /// <summary>
    /// Khởi tạo handler list messages.
    /// Luồng xử lý: nhận repository message/conversation/user/reader/finance và presence tracker để trả dữ liệu tin nhắn đầy đủ ngữ cảnh.
    /// </summary>
    public ListMessagesQueryHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IUserRepository userRepo,
        IReaderProfileRepository readerProfileRepository,
        IChatFinanceRepository financeRepository,
        IUserPresenceTracker presenceTracker,
        IConversationReviewRepository conversationReviewRepository)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
        _readerProfileRepository = readerProfileRepository;
        _financeRepository = financeRepository;
        _presenceTracker = presenceTracker;
        _conversationReviewRepository = conversationReviewRepository;
    }

    /// <summary>
    /// Xử lý query lấy message theo cursor.
    /// Luồng xử lý: kiểm tra quyền truy cập conversation, lấy page message, enrich profile + reader status + escrow, rồi trả kết quả.
    /// </summary>
    public async Task<ListMessagesResult> Handle(ListMessagesQuery request, CancellationToken cancellationToken)
    {
        var conversation = await LoadAuthorizedConversationAsync(request, cancellationToken);
        var (items, nextCursor) = await _messageRepo.GetByConversationIdCursorAsync(
            request.ConversationId,
            request.Cursor,
            request.Limit,
            cancellationToken);

        // Enrich conversation để client không cần gọi thêm API phụ trợ.
        await EnrichParticipantProfilesAsync(conversation, cancellationToken);
        await EnrichReaderStatusAndEscrowAsync(conversation, cancellationToken);
        await EnrichConversationReviewStateAsync(conversation, request.RequesterId.ToString(), cancellationToken);

        return new ListMessagesResult
        {
            Messages = items,
            NextCursor = nextCursor,
            Conversation = conversation
        };
    }
}
