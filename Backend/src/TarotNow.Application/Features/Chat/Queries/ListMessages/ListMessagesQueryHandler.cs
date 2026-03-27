using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Queries.ListMessages;

public partial class ListMessagesQueryHandler : IRequestHandler<ListMessagesQuery, ListMessagesResult>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;
    private readonly IUserRepository _userRepo;
    private readonly IReaderProfileRepository _readerProfileRepository;
    private readonly IChatFinanceRepository _financeRepository;

    public ListMessagesQueryHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo,
        IUserRepository userRepo,
        IReaderProfileRepository readerProfileRepository,
        IChatFinanceRepository financeRepository)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
        _userRepo = userRepo;
        _readerProfileRepository = readerProfileRepository;
        _financeRepository = financeRepository;
    }

    public async Task<ListMessagesResult> Handle(ListMessagesQuery request, CancellationToken cancellationToken)
    {
        var conversation = await LoadAuthorizedConversationAsync(request, cancellationToken);
        var (items, nextCursor) = await _messageRepo.GetByConversationIdCursorAsync(
            request.ConversationId,
            request.Cursor,
            request.Limit,
            cancellationToken);

        await EnrichParticipantProfilesAsync(conversation, cancellationToken);
        await EnrichReaderStatusAndEscrowAsync(conversation, cancellationToken);

        return new ListMessagesResult
        {
            Messages = items,
            NextCursor = nextCursor,
            Conversation = conversation
        };
    }
}
