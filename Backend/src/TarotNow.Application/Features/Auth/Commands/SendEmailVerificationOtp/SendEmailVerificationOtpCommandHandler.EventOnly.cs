using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

public sealed class SendEmailVerificationOtpCommandHandler : IRequestHandler<SendEmailVerificationOtpCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    public SendEmailVerificationOtpCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    public async Task<bool> Handle(SendEmailVerificationOtpCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new SendEmailVerificationOtpCommandHandlerRequestedDomainEvent(request);
        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Result is null ? default! : (bool)domainEvent.Result;
    }
}

public sealed class SendEmailVerificationOtpCommandHandlerRequestedDomainEvent : IDomainEvent
{
    public SendEmailVerificationOtpCommand Command { get; }

    public object? Result { get; set; }

    public DateTime OccurredAtUtc { get; } = DateTime.UtcNow;

    public SendEmailVerificationOtpCommandHandlerRequestedDomainEvent(SendEmailVerificationOtpCommand command)
    {
        Command = command;
    }
}
