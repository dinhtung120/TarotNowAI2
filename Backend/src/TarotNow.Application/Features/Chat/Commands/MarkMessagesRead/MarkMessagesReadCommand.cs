

using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Features.Chat.Commands.MarkMessagesRead;

public class MarkMessagesReadCommand : IRequest<bool>
{
        public string ConversationId { get; set; } = string.Empty;

        public Guid ReaderId { get; set; }
}

public class MarkMessagesReadCommandHandler : IRequestHandler<MarkMessagesReadCommand, bool>
{
    private readonly IConversationRepository _conversationRepo;
    private readonly IChatMessageRepository _messageRepo;

    public MarkMessagesReadCommandHandler(
        IConversationRepository conversationRepo,
        IChatMessageRepository messageRepo)
    {
        _conversationRepo = conversationRepo;
        _messageRepo = messageRepo;
    }

    public async Task<bool> Handle(MarkMessagesReadCommand request, CancellationToken cancellationToken)
    {
        
        var conversation = await _conversationRepo.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var readerId = request.ReaderId.ToString();

        
        
        if (conversation.UserId != readerId && conversation.ReaderId != readerId)
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");

        
        await _messageRepo.MarkAsReadAsync(request.ConversationId, readerId, cancellationToken);

        
        
        if (readerId == conversation.UserId)
            conversation.UnreadCountUser = 0;
        else
            conversation.UnreadCountReader = 0;

        await _conversationRepo.UpdateAsync(conversation, cancellationToken);

        return true;
    }
}
