using TarotNow.Domain.Entities;

namespace TarotNow.Application.Interfaces;

/// <summary>
/// Service dựng prompt reading/follow-up từ runtime system config.
/// </summary>
public interface IReadingPromptService
{
    /// <summary>
    /// Dựng cặp system prompt + user prompt cho luồng stream reading.
    /// </summary>
    ReadingPromptBundle Build(
        ReadingSession session,
        string? followupQuestion,
        string language);
}

/// <summary>
/// Cặp prompt đã resolve cho provider AI.
/// </summary>
public sealed class ReadingPromptBundle
{
    public required string SystemPrompt { get; init; }

    public required string UserPrompt { get; init; }

    public required string ResolvedLanguage { get; init; }
}
