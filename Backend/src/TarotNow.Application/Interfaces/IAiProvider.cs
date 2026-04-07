

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace TarotNow.Application.Interfaces;

public interface IAiProvider
{
        IAsyncEnumerable<string> StreamChatAsync(string systemPrompt, string userPrompt, CancellationToken cancellationToken);
    
        string ProviderName { get; }

        string ModelName { get; }

        Task LogRequestAsync(AiProviderRequestLog logEntry, CancellationToken cancellationToken = default);
}

public sealed class AiProviderRequestLog
{
    public Guid UserId { get; init; }
    public string? SessionId { get; init; }
    public string? RequestId { get; init; }
    public int InputTokens { get; init; }
    public int OutputTokens { get; init; }
    public int LatencyMs { get; init; }
    public string Status { get; init; } = "requested";
    public string? ErrorCode { get; init; }
}
