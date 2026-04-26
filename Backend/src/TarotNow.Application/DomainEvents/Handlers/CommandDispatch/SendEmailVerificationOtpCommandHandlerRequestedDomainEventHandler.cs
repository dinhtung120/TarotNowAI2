using System;
using System.Threading;
using System.Threading.Tasks;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Application.Features.Auth.Commands.SendEmailVerificationOtp;

namespace TarotNow.Application.DomainEvents.Handlers.CommandDispatch;

public sealed class SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<SendEmailVerificationOtpCommandHandlerRequestedDomainEvent>
{
    private readonly ICommandExecutionExecutor<SendEmailVerificationOtpCommand, bool> _executor;

    public SendEmailVerificationOtpCommandHandlerRequestedDomainEventHandler(
        ICommandExecutionExecutor<SendEmailVerificationOtpCommand, bool> executor,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _executor = executor;
    }

    protected override async Task HandleDomainEventAsync(
        SendEmailVerificationOtpCommandHandlerRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        domainEvent.Result = await _executor.Handle(domainEvent.Command, cancellationToken);
    }
}
