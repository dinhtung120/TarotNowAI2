using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;
using TarotNow.Application.Common.MediaUpload;

namespace TarotNow.Application.Features.Profile.Commands.PresignAvatarUpload;

public sealed class PresignAvatarUploadCommandHandler : IRequestHandler<PresignAvatarUploadCommand, PresignedUploadResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public PresignAvatarUploadCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<PresignedUploadResult> Handle(PresignAvatarUploadCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new PresignAvatarUploadCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (PresignedUploadResult)domainEvent.Result;
    }
}

public sealed class PresignAvatarUploadCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public PresignAvatarUploadCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public PresignAvatarUploadCommandHandlerRequestedDomainEvent(PresignAvatarUploadCommand command)
    {
        Command = command;
    }
}
