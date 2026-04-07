

using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.CreateConversation;

public partial class CreateConversationCommandHandler : IRequestHandler<CreateConversationCommand, ConversationDto>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IReaderProfileRepository _readerProfileRepo;

    public CreateConversationCommandHandler(
        IConversationRepository conversationRepo,
        IReaderProfileRepository readerProfileRepo)
    {
        _conversationRepo = conversationRepo;
        _readerProfileRepo = readerProfileRepo;
    }

    public async Task<ConversationDto> Handle(CreateConversationCommand request, CancellationToken cancellationToken)
    {
        ValidateRequest(request);
        await EnsureReaderIsAvailableAsync(request, cancellationToken);
        await EnsureUserActiveLimitAsync(request, cancellationToken);

        var existing = await _conversationRepo.GetActiveByParticipantsAsync(
            request.UserId.ToString(),
            request.ReaderId.ToString(),
            cancellationToken);
        if (existing != null)
        {
            return existing;
        }

        var conversation = BuildConversation(request);
        await _conversationRepo.AddAsync(conversation, cancellationToken);
        return conversation;
    }
}
