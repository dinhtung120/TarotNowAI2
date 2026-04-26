using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Profile.Commands.ConfirmAvatarUpload;

public sealed class ConfirmAvatarUploadCommandHandler : IRequestHandler<ConfirmAvatarUploadCommand, ConfirmAvatarUploadResult>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public ConfirmAvatarUploadCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<ConfirmAvatarUploadResult> Handle(ConfirmAvatarUploadCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new ConfirmAvatarUploadCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (ConfirmAvatarUploadResult)domainEvent.Result;
    }
}

public sealed class ConfirmAvatarUploadCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public ConfirmAvatarUploadCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public ConfirmAvatarUploadCommandHandlerRequestedDomainEvent(ConfirmAvatarUploadCommand command)
    {
        Command = command;
    }
}
