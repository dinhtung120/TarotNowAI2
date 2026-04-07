using MediatR;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.GetConversationParticipants;

public class GetConversationParticipantsQuery : IRequest<ConversationParticipantsDto?>
{
    public string ConversationId { get; set; } = string.Empty;
}

public class ConversationParticipantsDto
{
    public string UserId { get; set; } = string.Empty;
    public string ReaderId { get; set; } = string.Empty;
}

public class GetConversationParticipantsQueryHandler
    : IRequestHandler<GetConversationParticipantsQuery, ConversationParticipantsDto?>
{
    private readonly IConversationRepository _conversationRepository;

    public GetConversationParticipantsQueryHandler(IConversationRepository conversationRepository)
    {
        _conversationRepository = conversationRepository;
    }

    public async Task<ConversationParticipantsDto?> Handle(
        GetConversationParticipantsQuery request,
        CancellationToken cancellationToken)
    {
        var conversation = await _conversationRepository.GetByIdAsync(
            request.ConversationId,
            cancellationToken);

        if (conversation == null)
        {
            return null;
        }

        return new ConversationParticipantsDto
        {
            UserId = conversation.UserId,
            ReaderId = conversation.ReaderId
        };
    }
}
