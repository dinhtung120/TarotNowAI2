using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

/// <summary>
/// Query lấy lịch sử tin nhắn trong conversation.
/// Phân trang, sort by created_at DESC (frontend sẽ reverse).
/// </summary>
public class ListMessagesQuery : IRequest<ListMessagesResult>
{
    /// <summary>ObjectId conversation.</summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>UUID user yêu cầu — kiểm tra quyền.</summary>
    public Guid RequesterId { get; set; }

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 50;
}

public class ListMessagesResult
{
    public IEnumerable<ChatMessageDto> Messages { get; set; } = Enumerable.Empty<ChatMessageDto>();
    public long TotalCount { get; set; }
}

/// <summary>
/// Handler lấy tin nhắn — kiểm tra quyền truy cập conversation.
/// </summary>
public class ListMessagesQueryHandler : IRequestHandler<ListMessagesQuery, ListMessagesResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public ListMessagesQueryHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<ListMessagesResult> Handle(ListMessagesQuery request, CancellationToken cancellationToken)
    {
        // 1. Kiểm tra conversation tồn tại
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        // 2. Kiểm tra quyền — phải là member
        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        // 3. Lấy tin nhắn phân trang
        var (items, totalCount) = await _messageRepo.GetByConversationIdPaginatedAsync(
            request.ConversationId, request.Page, request.PageSize, cancellationToken);

        return new ListMessagesResult
        {
            Messages = items,
            TotalCount = totalCount
        };
    }
}
