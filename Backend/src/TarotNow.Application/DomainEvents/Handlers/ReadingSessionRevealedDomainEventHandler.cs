using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Common.DomainEvents;
using TarotNow.Application.Features.Reading.Commands.CompleteAiStream;
using TarotNow.Application.Features.Reading.Commands.StreamReading;
using TarotNow.Application.Interfaces.DomainEvents;
using TarotNow.Domain.Events;

namespace TarotNow.Application.DomainEvents.Handlers;

/// <summary>
/// Handler hậu reveal để precompute AI ở nền.
/// </summary>
public sealed class ReadingSessionRevealedDomainEventHandler
    : IdempotentDomainEventNotificationHandler<ReadingSessionRevealedDomainEvent>
{
    private readonly IMediator _mediator;
    private readonly ILogger<ReadingSessionRevealedDomainEventHandler> _logger;

    /// <summary>
    /// Khởi tạo handler session-revealed.
    /// </summary>
    public ReadingSessionRevealedDomainEventHandler(
        IMediator mediator,
        ILogger<ReadingSessionRevealedDomainEventHandler> logger,
        IEventHandlerIdempotencyService idempotencyService)
        : base(idempotencyService)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task HandleDomainEventAsync(
        ReadingSessionRevealedDomainEvent domainEvent,
        Guid? outboxMessageId,
        CancellationToken cancellationToken)
    {
        var streamResult = await TryStartPrecomputeAsync(domainEvent, cancellationToken);
        if (streamResult is null)
        {
            return;
        }

        var streamState = await ConsumeStreamAsync(streamResult, domainEvent, cancellationToken);
        await CompleteStreamAsync(streamResult, streamState, domainEvent, cancellationToken);
    }

    private async Task<StreamReadingResult?> TryStartPrecomputeAsync(
        ReadingSessionRevealedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        try
        {
            return await _mediator.Send(
                new StreamReadingCommand
                {
                    UserId = domainEvent.UserId,
                    ReadingSessionId = domainEvent.SessionId,
                    FollowupQuestion = null,
                    Language = NormalizeLanguage(domainEvent.Language)
                },
                cancellationToken);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(
                ex,
                "Failed to start background AI precompute. SessionId={SessionId} UserId={UserId}",
                domainEvent.SessionId,
                domainEvent.UserId);
            return null;
        }
    }

    private async Task<PrecomputeStreamState> ConsumeStreamAsync(
        StreamReadingResult streamResult,
        ReadingSessionRevealedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        var state = new PrecomputeStreamState();

        try
        {
            await foreach (var streamChunk in streamResult.Stream.WithCancellation(cancellationToken))
            {
                if (streamChunk.Usage is not null)
                {
                    state.ProviderOutputTokens = Math.Max(0, streamChunk.Usage.OutputTokens);
                    if (streamChunk.Usage.InputTokens > 0)
                    {
                        state.ProviderInputTokens = streamChunk.Usage.InputTokens;
                    }
                }

                if (string.IsNullOrWhiteSpace(streamChunk.Content))
                {
                    continue;
                }

                state.FirstTokenAt ??= DateTimeOffset.UtcNow;
                state.FullResponse.Append(streamChunk.Content);
                state.EstimatedOutputTokens += EstimateTokenCount(streamChunk.Content);
            }
        }
        catch (Exception ex)
        {
            state.FinalStatus = state.FirstTokenAt.HasValue
                ? AiStreamFinalStatuses.FailedAfterFirstToken
                : AiStreamFinalStatuses.FailedBeforeFirstToken;
            state.ErrorMessage = ex.Message;

            _logger.LogWarning(
                ex,
                "Background AI precompute stream failed. SessionId={SessionId} UserId={UserId}",
                domainEvent.SessionId,
                domainEvent.UserId);
        }

        return state;
    }

    private Task CompleteStreamAsync(
        StreamReadingResult streamResult,
        PrecomputeStreamState state,
        ReadingSessionRevealedDomainEvent domainEvent,
        CancellationToken cancellationToken)
    {
        return _mediator.Send(
            new CompleteAiStreamCommand
            {
                AiRequestId = streamResult.AiRequestId,
                UserId = domainEvent.UserId,
                FinalStatus = state.FinalStatus,
                ErrorMessage = state.ErrorMessage,
                IsClientDisconnect = false,
                FirstTokenAt = state.FirstTokenAt,
                OutputTokens = state.OutputTokens,
                InputTokens = state.ResolveInputTokens(streamResult.EstimatedInputTokens),
                LatencyMs = 0,
                FullResponse = state.FullResponse.ToString(),
                FollowupQuestion = null
            },
            cancellationToken);
    }

    private static string NormalizeLanguage(string? language)
    {
        return language?.Trim().ToLowerInvariant() switch
        {
            "en" => "en",
            "zh" => "zh",
            _ => "vi"
        };
    }

    private sealed class PrecomputeStreamState
    {
        public DateTimeOffset? FirstTokenAt { get; set; }

        public int EstimatedOutputTokens { get; set; }

        public int? ProviderOutputTokens { get; set; }

        public int? ProviderInputTokens { get; set; }

        public string FinalStatus { get; set; } = AiStreamFinalStatuses.Completed;

        public string? ErrorMessage { get; set; }

        public System.Text.StringBuilder FullResponse { get; } = new();

        public int OutputTokens => ProviderOutputTokens ?? EstimatedOutputTokens;

        public int ResolveInputTokens(int fallbackInputTokens)
        {
            return ProviderInputTokens ?? fallbackInputTokens;
        }
    }

    private static int EstimateTokenCount(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return 0;
        }

        var normalizedLength = content.Trim().Length;
        return Math.Max(1, (int)Math.Ceiling(normalizedLength / 4d));
    }
}
