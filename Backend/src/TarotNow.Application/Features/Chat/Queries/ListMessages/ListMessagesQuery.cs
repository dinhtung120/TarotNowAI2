

using MediatR;
using TarotNow.Application.Common;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public class ListMessagesQuery : IRequest<ListMessagesResult>
{
        public string ConversationId { get; set; } = string.Empty;

        public Guid RequesterId { get; set; }

        public string? Cursor { get; set; }

        public int Limit { get; set; } = 50;
}

public class ListMessagesResult
{
    public IReadOnlyList<ChatMessageDto> Messages { get; set; } = Array.Empty<ChatMessageDto>();
    public string? NextCursor { get; set; }
    
    
    public ConversationDto Conversation { get; set; } = null!;
}
