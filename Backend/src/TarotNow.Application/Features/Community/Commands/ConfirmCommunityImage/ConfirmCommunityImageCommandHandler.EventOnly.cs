using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Community.Commands.ConfirmCommunityImage;

public sealed class ConfirmCommunityImageCommandHandler : IRequestHandler<ConfirmCommunityImageCommand, ConfirmCommunityImageResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ConfirmCommunityImageCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConfirmCommunityImageResult> Handle(ConfirmCommunityImageCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ConfirmCommunityImageCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConfirmCommunityImageResult)domainEvent.Result;
    }
}

public sealed class ConfirmCommunityImageCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ConfirmCommunityImageCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ConfirmCommunityImageCommandHandlerRequestedDomainEvent(ConfirmCommunityImageCommand command)
    {
        Command = command;
    }
}
