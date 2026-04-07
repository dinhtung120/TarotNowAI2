using TarotNow.Application.Interfaces;

namespace TarotNow.Infrastructure.Services.Ai;

public partial class OpenAiProvider
{
    public async Task LogRequestAsync(
        AiProviderRequestLog logEntry,
        CancellationToken cancellationToken = default)
    {
        try
        {
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
        catch
        {
            
        }
    }
}
