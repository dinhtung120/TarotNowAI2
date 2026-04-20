using MediatR;
using TarotNow.Application.Exceptions;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Chat.Commands.PublishTypingState;

/// <summary>
/// Command phát typing state của participant trong conversation.
/// </summary>
public sealed class PublishTypingStateCommand : IRequest<bool>
{
    /// <summary>
    /// Định danh conversation.
    /// </summary>
    public string ConversationId { get; set; } = string.Empty;

    /// <summary>
    /// Định danh participant phát typing state.
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// True nếu bắt đầu gõ, false nếu dừng gõ.
    /// </summary>
    public bool IsTyping { get; set; }
}

/// <summary>
/// Handler xử lý publish typing state qua domain event.
/// </summary>
public sealed class PublishTypingStateCommandHandler
    : IRequestHandler<PublishTypingStateCommand, bool>
{
    private readonly IConversationRepository _conversationRepository;
    private readonly IDomainEventPublisher _domainEventPublisher;

    /// <summary>
    /// Khởi tạo handler publish typing state.
    /// </summary>
    public PublishTypingStateCommandHandler(
        IConversationRepository conversationRepository,
        IDomainEventPublisher domainEventPublisher)
    {
        _conversationRepository = conversationRepository;
        _domainEventPublisher = domainEventPublisher;
    }

    /// <inheritdoc />
    public async Task<bool> Handle(PublishTypingStateCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.ConversationId))
        {
            throw new BadRequestException("ConversationId is required.");
        }

        var conversation = await _conversationRepository.GetByIdAsync(request.ConversationId, cancellationToken)
            ?? throw new NotFoundException("Không tìm thấy cuộc trò chuyện.");

        var userId = request.UserId.ToString();
        if (conversation.UserId != userId && conversation.ReaderId != userId)
        {
            throw new BadRequestException("Bạn không phải thành viên của cuộc trò chuyện này.");
        }

        await _domainEventPublisher.PublishAsync(
            new ChatTypingChangedDomainEvent
            {
                ConversationId = request.ConversationId,
                UserId = userId,
                IsTyping = request.IsTyping,
                OccurredAtUtc = DateTime.UtcNow
            },
            cancellationToken);

        return true;
    }
}
