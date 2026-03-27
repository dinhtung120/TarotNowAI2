using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ValidateConversationAccess;

public class ValidateConversationAccessQuery : IRequest<ConversationAccessStatus>
{
    public string ConversationId { get; set; } = string.Empty;
    public Guid RequesterId { get; set; }
}

public enum ConversationAccessStatus
{
    Allowed = 1,
    NotFound = 2,
    Forbidden = 3
}

public class ValidateConversationAccessQueryHandler
    : IRequestHandler<ValidateConversationAccessQuery, ConversationAccessStatus>
{
    private readonly IConversationRepository _conversationRepository;

    public ValidateConversationAccessQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<ConversationAccessStatus> Handle(
        ValidateConversationAccessQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken);
        if (conversation == null)
        {
            return ConversationAccessStatus.NotFound;
        }

        var requesterId = request.RequesterId.ToString();
        if (conversation.UserId != requesterId && conversation.ReaderId != requesterId)
        {
            return ConversationAccessStatus.Forbidden;
        }

        return ConversationAccessStatus.Allowed;
    }
}
