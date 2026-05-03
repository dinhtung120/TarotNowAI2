using MediatR;
using Microsoft.Extensions.Logging;
using TarotNow.Application.Interfaces;

namespace TarotNow.Application.Behaviors;

/// <summary>
/// Pipeline behavior bọc command trong transaction thống nhất để bảo toàn atomicity business write + outbox.
/// </summary>
public sealed class CommandTransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private const int ChatOutboxInlineFlushPasses = 1;
    private const int DeepChatOutboxInlineFlushPasses = 2;

    private readonly ITransactionCoordinator _transactionCoordinator;
    private readonly IOutboxBatchProcessor _outboxBatchProcessor;
    private readonly ILogger<CommandTransactionBehavior<TRequest, TResponse>> _logger;

    /// <summary>
    /// Khởi tạo behavior transaction cho command pipeline.
    /// </summary>
    public CommandTransactionBehavior(
        ITransactionCoordinator transactionCoordinator,
        IOutboxBatchProcessor outboxBatchProcessor,
        ILogger<CommandTransactionBehavior<TRequest, TResponse>> logger)
    {
        _transactionCoordinator = transactionCoordinator;
        _outboxBatchProcessor = outboxBatchProcessor;
        _logger = logger;
    }

    /// <inheritdoc />
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is INonTransactionalCommand)
        {
            return await next();
        }

        if (IsCommandRequest() == false)
        {
            return await next();
        }

        var response = default(TResponse);
        await _transactionCoordinator.ExecuteAsync(
            async _ => response = await next(),
            cancellationToken);

        if (IsChatCommandRequest())
        {
            await FlushChatOutboxInlineAsync(cancellationToken);
        }

        return response!;
    }

    private static bool IsCommandRequest()
    {
        var requestType = typeof(TRequest);
        var requestNamespace = requestType.Namespace ?? string.Empty;
        return requestNamespace.Contains(".Commands.", StringComparison.Ordinal)
               || requestType.Name.EndsWith("Command", StringComparison.Ordinal);
    }

    private static bool IsChatCommandRequest()
    {
        var requestType = typeof(TRequest);
        var requestNamespace = requestType.Namespace ?? string.Empty;
        return requestNamespace.Contains(".Features.Chat.Commands.", StringComparison.Ordinal);
    }

    private async Task FlushChatOutboxInlineAsync(CancellationToken cancellationToken)
    {
        var maxPasses = ShouldFlushDeepChatOutbox() ? DeepChatOutboxInlineFlushPasses : ChatOutboxInlineFlushPasses;

        for (var pass = 0; pass < maxPasses; pass++)
        {
            var processed = await _outboxBatchProcessor.ProcessOnceAsync(cancellationToken);
            if (processed <= 0)
            {
                return;
            }

            _logger.LogDebug(
                "Inline chat outbox flush processed {ProcessedCount} message(s) on pass {Pass}.",
                processed,
                pass + 1);
        }
    }

    private static bool ShouldFlushDeepChatOutbox()
    {
        return typeof(TRequest).Name.Equals("RespondConversationAddMoneyCommand", StringComparison.Ordinal);
    }
}
