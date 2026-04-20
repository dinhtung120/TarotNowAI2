using MediatR;
using TarotNow.Application.Interfaces;
using TarotNow.Domain.Events;

namespace TarotNow.Application.Features.Deposit.Commands.ProcessDepositWebhook;

// Handler xử lý webhook theo Rule 0: chỉ publish domain event.
public class ProcessDepositWebhookCommandHandler : IRequestHandler<ProcessDepositWebhookCommand, bool>
{
    private readonly IInlineDomainEventDispatcher _inlineDomainEventDispatcher;

    /// <summary>
    /// Khởi tạo handler process deposit webhook.
    /// </summary>
    public ProcessDepositWebhookCommandHandler(IInlineDomainEventDispatcher inlineDomainEventDispatcher)
    {
        _inlineDomainEventDispatcher = inlineDomainEventDispatcher;
    }

    /// <summary>
    /// Xử lý command webhook bằng cách publish domain event.
    /// </summary>
    public async Task<bool> Handle(ProcessDepositWebhookCommand request, CancellationToken cancellationToken)
    {
        var domainEvent = new DepositWebhookReceivedDomainEvent
        {
            RawPayload = request.RawPayload
        };

        await _inlineDomainEventDispatcher.PublishAsync(domainEvent, cancellationToken);
        return domainEvent.Handled;
    }
}
