using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed class ChatModerationQueue : IChatModerationQueue
{
    private readonly Channel<ChatModerationPayload> _channel;
    private readonly ILogger<ChatModerationQueue> _logger;

    public ChatModerationQueue(
        IOptions<ChatModerationOptions> options,
        ILogger<ChatModerationQueue> logger)
    {
        _logger = logger;

        var capacity = Math.Max(100, options.Value.MaxQueueSize);
        _channel = Channel.CreateBounded<ChatModerationPayload>(new BoundedChannelOptions(capacity)
        {
            SingleReader = true,
            SingleWriter = false,
            FullMode = BoundedChannelFullMode.DropOldest
        });
    }

    public ValueTask EnqueueAsync(ChatModerationPayload payload, CancellationToken cancellationToken = default)
    {
        if (_channel.Writer.TryWrite(payload))
        {
            return ValueTask.CompletedTask;
        }

        _logger.LogWarning(
            "[ChatModerationQueue] Queue is full. Dropping oldest before enqueue. ConversationId={ConversationId}, MessageId={MessageId}",
            payload.ConversationId,
            payload.MessageId);

        return _channel.Writer.WriteAsync(payload, cancellationToken);
    }

    public IAsyncEnumerable<ChatModerationPayload> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
