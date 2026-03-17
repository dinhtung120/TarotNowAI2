using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListConversations;

/// <summary>
/// Query lấy danh sách conversations (inbox) cho user hoặc reader.
/// Phân trang, sort by last_message_at DESC.
/// </summary>
public class ListConversationsQuery : IRequest<ListConversationsResult>
{
    /// <summary>UUID user hiện tại.</summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Vai trò trong query: "user" hoặc "reader".
    /// Xác định query theo user_id hay reader_id.
    /// </summary>
    public string Role { get; set; } = "user";

    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class ListConversationsResult
{
    public IEnumerable<ConversationDto> Conversations { get; set; } = Enumerable.Empty<ConversationDto>();
    public long TotalCount { get; set; }
}

/// <summary>
/// Handler phân trang inbox — delegate sang repository.
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

        // Phân biệt query dựa trên role
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
