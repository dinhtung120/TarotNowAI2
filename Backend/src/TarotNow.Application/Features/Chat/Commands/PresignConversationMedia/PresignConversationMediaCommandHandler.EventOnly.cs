using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Chat.Commands.PresignConversationMedia;

public sealed class PresignConversationMediaCommandHandler : IRequestHandler<PresignConversationMediaCommand, PresignedUploadResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PresignConversationMediaCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<PresignedUploadResult> Handle(PresignConversationMediaCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PresignConversationMediaCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (PresignedUploadResult)domainEvent.Result;
    }
}

public sealed class PresignConversationMediaCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public PresignConversationMediaCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public PresignConversationMediaCommandHandlerRequestedDomainEvent(PresignConversationMediaCommand command)
    {
        Command = command;
    }
}
