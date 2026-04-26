using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Community.Commands.PresignCommunityImage;

public sealed class PresignCommunityImageCommandHandler : IRequestHandler<PresignCommunityImageCommand, PresignedUploadResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PresignCommunityImageCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<PresignedUploadResult> Handle(PresignCommunityImageCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PresignCommunityImageCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (PresignedUploadResult)domainEvent.Result;
    }
}

public sealed class PresignCommunityImageCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public PresignCommunityImageCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public PresignCommunityImageCommandHandlerRequestedDomainEvent(PresignCommunityImageCommand command)
    {
        Command = command;
    }
}
