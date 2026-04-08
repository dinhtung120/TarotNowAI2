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

    /// <summary>
    /// Khởi tạo hàng đợi moderation dạng bounded channel để kiểm soát áp lực tải.
    /// Luồng xử lý: đọc MaxQueueSize từ options, ép tối thiểu 100, tạo channel single-reader với chiến lược DropOldest.
    /// </summary>
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

    /// <summary>
    /// Đưa payload moderation vào hàng đợi.
    /// Luồng xử lý: ưu tiên TryWrite nhanh; nếu đầy thì log warning và ghi theo cơ chế channel (DropOldest).
    /// </summary>
    public ValueTask EnqueueAsync(ChatModerationPayload payload, CancellationToken cancellationToken = default)
    {
        if (_channel.Writer.TryWrite(payload))
        {
            // Nhánh bình thường: enqueue thành công ngay, không chặn thread gọi.
            return ValueTask.CompletedTask;
        }

        _logger.LogWarning(
            "[ChatModerationQueue] Queue is full. Dropping oldest before enqueue. ConversationId={ConversationId}, MessageId={MessageId}",
            payload.ConversationId,
            payload.MessageId);

        // Queue đầy: writer sẽ áp dụng policy DropOldest để giữ payload mới nhất.
        return _channel.Writer.WriteAsync(payload, cancellationToken);
    }

    /// <summary>
    /// Trả stream payload moderation để worker tiêu thụ liên tục.
    /// Luồng xử lý: ủy quyền cho channel reader đọc toàn bộ item theo cancellation token.
    /// </summary>
    public IAsyncEnumerable<ChatModerationPayload> ReadAllAsync(CancellationToken cancellationToken)
    {
        return _channel.Reader.ReadAllAsync(cancellationToken);
    }
}
