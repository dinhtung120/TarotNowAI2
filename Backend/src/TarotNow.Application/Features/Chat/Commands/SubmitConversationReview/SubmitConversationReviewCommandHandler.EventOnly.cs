using MediatR;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Chat.Commands.SubmitConversationReview;

/// <summary>
/// Command handler mỏng: chỉ publish requested domain event cho workflow review.
/// </summary>
public sealed class SubmitConversationReviewCommandHandler : IRequestHandler<SubmitConversationReviewCommand, ConversationReviewSubmitResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo command handler submit review.
    /// </summary>
    public SubmitConversationReviewCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <inheritdoc />
    public async Task<ConversationReviewSubmitResult> Handle(
        SubmitConversationReviewCommand request,
        CancellationToken cancellationToken)
    {
        var domainEvent = new SubmitConversationReviewCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConversationReviewSubmitResult)domainEvent.Result;
    }
}

/// <summary>
/// Requested domain event chứa payload command submit conversation review.
/// </summary>
public sealed class SubmitConversationReviewCommandHandlerRequestedDomainEvent : IDomainEvent
{
    /// <summary>
    /// Command submit review cần xử lý.
    /// </summary>
    public SubmitConversationReviewCommand Command { get; }

    /// <summary>
    /// Kết quả xử lý command được hydrate bởi workflow handler.
    /// </summary>
    public object? Result { get; set; }

    /// <inheritdoc />
    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    /// <summary>
    /// Khởi tạo requested event cho luồng submit conversation review.
    /// </summary>
    public SubmitConversationReviewCommandHandlerRequestedDomainEvent(SubmitConversationReviewCommand command)
    {
        Command = command;
    }
}
