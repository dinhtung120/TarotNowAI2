using TarotNow.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace TarotNow.Infrastructure.Services.Ai;

public partial class OpenAiProvider
{
    /// <summary>
    /// Ghi log metadata của request AI để phục vụ quan sát vận hành và đối soát chi phí.
    /// Luồng này không được phép làm hỏng luồng chính nên lỗi log sẽ bị nuốt có chủ đích.
    /// </summary>
    public async Task LogRequestAsync(
        AiProviderRequestLog logEntry,
        CancellationToken cancellationToken = default)
    {
        try
        {
            // Map DTO log để lưu nhất quán schema telemetry của provider.
            await _logRepo.CreateAsync(new AiProviderLogCreateDto
            {
                UserId = logEntry.UserId,
                ReadingRef = logEntry.SessionId,
                AiRequestRef = logEntry.RequestId,
                Model = _modelName,
                InputTokens = logEntry.InputTokens,
                OutputTokens = logEntry.OutputTokens,
                LatencyMs = logEntry.LatencyMs,
                Status = logEntry.Status,
                ErrorCode = logEntry.ErrorCode,
                PromptVersion = "v1.0"
            }, cancellationToken);
        }
        catch (Exception exception)
        {
            _logger.LogWarning(
                exception,
                "Failed to write AI provider telemetry. RequestId={RequestId}, UserId={UserId}",
                logEntry.RequestId,
                logEntry.UserId);
        }
    }
}
