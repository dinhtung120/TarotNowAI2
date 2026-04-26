using Microsoft.Extensions.Logging;
using TarotNow.Application.Common;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Interfaces;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler ghi telemetry completion của AI stream qua provider theo cơ chế event-driven.
/// </summary>
public sealed class AiStreamCompletionTelemetryRequestedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<AiStreamCompletionTelemetryRequestedDomainEvent>
{
    private readonly IAiProvider _aiProvider;
    private readonly ILogger<AiStreamCompletionTelemetryRequestedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler telemetry AI stream.
    /// </summary>
    public AiStreamCompletionTelemetryRequestedDomainEventHandler(
        IAiProvider aiProvider,
        ILogger<AiStreamCompletionTelemetryRequestedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _aiProvider = aiProvider;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        AiStreamCompletionTelemetryRequestedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        try
        {
            await _aiProvider.LogRequestAsync(new AiProviderRequestLog
            {
                UserId = domainEvent.UserId,
                SessionId = domainEvent.SessionId,
                RequestId = domainEvent.RequestId,
                InputTokens = 0,
                OutputTokens = domainEvent.OutputTokens,
                LatencyMs = domainEvent.LatencyMs,
                Status = domainEvent.Status,
                ErrorCode = domainEvent.ErrorCode
            });
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "AI completion telemetry failed for request {AiRequestId}, user {UserId}.",
                domainEvent.AiRequestId,
                domainEvent.UserId);
        }
    }
}
