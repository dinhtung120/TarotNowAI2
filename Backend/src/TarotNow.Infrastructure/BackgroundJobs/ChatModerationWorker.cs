using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.BackgroundJobs;

public sealed partial class ChatModerationWorker : BackgroundService
{
    private readonly ChatModerationQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IOptionsMonitor<ChatModerationOptions> _options;
    private readonly ILogger<ChatModerationWorker> _logger;

    /// <summary>
    /// Khởi tạo worker kiểm duyệt chat với queue, scope factory và cấu hình runtime.
    /// Luồng xử lý: nhận toàn bộ dependency qua DI để dùng trong vòng lặp ExecuteAsync.
    /// </summary>
    public ChatModerationWorker(
        ChatModerationQueue queue,
        IServiceScopeFactory scopeFactory,
        IOptionsMonitor<ChatModerationOptions> options,
        ILogger<ChatModerationWorker> logger)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
        _options = options;
        _logger = logger;
    }

    /// <summary>
    /// Vòng lặp worker đọc payload moderation từ queue và xử lý lần lượt.
    /// Luồng xử lý: đọc stream từ queue, bỏ qua khi feature tắt, xử lý từng payload với bắt lỗi cục bộ.
    /// </summary>
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("[ChatModerationWorker] Bắt đầu lắng nghe hàng đợi kiểm duyệt...");

        try
        {
            await foreach (var payload in _queue.ReadAllAsync(stoppingToken))
            {
                if (_options.CurrentValue.Enabled == false)
                {
                    // Feature flag tắt: bỏ qua payload để tránh tạo report ngoài ý muốn.
                    continue;
                }

                try
                {
                    await ProcessPayloadAsync(payload, stoppingToken);
                }
                catch (ObjectDisposedException) when (stoppingToken.IsCancellationRequested)
                {
                    // Bỏ qua lỗi dispose khi worker đang dừng có chủ đích.
                }
                catch (Exception ex) when (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogError(
                        ex,
                        "[ChatModerationWorker] Failed to process payload. ConversationId={ConversationId}, MessageId={MessageId}",
                        payload.ConversationId,
                        payload.MessageId);
                    // Giữ worker sống để tiếp tục xử lý payload khác dù một item lỗi.
                }
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogInformation("[ChatModerationWorker] Đang dừng công việc kiểm duyệt...");
        }

        _logger.LogInformation("[ChatModerationWorker] Tác vụ đã dừng.");
    }

    /// <summary>
    /// Xử lý một payload moderation: kiểm tra loại tin, match keyword, flag message và tạo report.
    /// Luồng xử lý: skip type không cần moderate, tìm keyword, cập nhật cờ message, lưu report tự động.
    /// </summary>
    private async Task ProcessPayloadAsync(ChatModerationPayload payload, CancellationToken cancellationToken)
    {
        if (ShouldModerate(payload.Type) == false)
        {
            // Chỉ moderate các loại tin có rủi ro cao; loại khác bỏ qua để tiết kiệm tài nguyên.
            return;
        }

        var matchedKeyword = MatchKeyword(payload.Content, _options.CurrentValue.Keywords);
        if (matchedKeyword == null)
        {
            // Không phát hiện keyword vi phạm thì không tạo report.
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var reportRepo = scope.ServiceProvider.GetRequiredService<IReportRepository>();
        var chatMessageRepo = scope.ServiceProvider.GetRequiredService<IChatMessageRepository>();

        await chatMessageRepo.UpdateFlagAsync(payload.MessageId, true, cancellationToken);
        // Đổi state message sang flagged để UI/admin có thể nhận diện ngay nội dung cần xem xét.

        var report = BuildAutoModerationReport(payload, matchedKeyword);
        await reportRepo.AddAsync(report, cancellationToken);
        // Ghi report moderation tự động để đưa vào quy trình xử lý của admin.

        _logger.LogWarning(
            "[ChatModerationWorker] Auto-flag created. MessageId={MessageId}, Keyword={Keyword}",
            payload.MessageId,
            matchedKeyword);
    }
}
