using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

/// <summary>
/// Persisted state machine cho reveal reading nhằm chống lệch dữ liệu cross-store khi retry/failure.
/// </summary>
public sealed class ReadingRevealSagaState
{
    public Guid Id { get; private set; }

    public string SessionId { get; private set; } = string.Empty;

    public Guid UserId { get; private set; }

    public string Language { get; private set; } = "vi";

    public string Status { get; private set; } = ReadingRevealSagaStatus.Processing;

    public string? RevealedCardsJson { get; private set; }

    public bool ChargeDebited { get; private set; }

    public string? ChargeCurrency { get; private set; }

    public string? ChargeChangeType { get; private set; }

    public long ChargeAmount { get; private set; }

    public string? ChargeReferenceId { get; private set; }

    public bool CollectionApplied { get; private set; }

    public bool ExpGranted { get; private set; }

    public bool SessionCompleted { get; private set; }

    public bool RevealedEventPublished { get; private set; }

    public bool RefundCompensated { get; private set; }

    public int AttemptCount { get; private set; }

    public DateTime? LastAttemptAtUtc { get; private set; }

    public DateTime? NextRepairAtUtc { get; private set; }

    public string? LastError { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime UpdatedAtUtc { get; private set; }

    public DateTime? CompletedAtUtc { get; private set; }

    private ReadingRevealSagaState()
    {
    }

    public static ReadingRevealSagaState Create(string sessionId, Guid userId, string language, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(sessionId))
        {
            throw new ArgumentException("SessionId is required.", nameof(sessionId));
        }

        return new ReadingRevealSagaState
        {
            Id = Guid.NewGuid(),
            SessionId = sessionId.Trim(),
            UserId = userId,
            Language = NormalizeLanguage(language),
            Status = ReadingRevealSagaStatus.Processing,
            CreatedAtUtc = nowUtc,
            UpdatedAtUtc = nowUtc
        };
    }

    public void StartAttempt(DateTime nowUtc)
    {
        AttemptCount += 1;
        LastAttemptAtUtc = nowUtc;
        Status = ReadingRevealSagaStatus.Processing;
        LastError = null;
        UpdatedAtUtc = nowUtc;
    }

    public void SetRevealedCards(string cardsJson, DateTime nowUtc)
    {
        if (string.IsNullOrWhiteSpace(cardsJson))
        {
            throw new ArgumentException("cardsJson is required.", nameof(cardsJson));
        }

        RevealedCardsJson = cardsJson;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkChargeDebited(
        string currency,
        string changeType,
        long amount,
        string referenceId,
        DateTime nowUtc)
    {
        ChargeDebited = true;
        ChargeCurrency = NormalizeOptional(currency);
        ChargeChangeType = NormalizeOptional(changeType);
        ChargeAmount = Math.Max(0, amount);
        ChargeReferenceId = NormalizeOptional(referenceId);
        UpdatedAtUtc = nowUtc;
    }

    public void MarkCollectionApplied(DateTime nowUtc)
    {
        CollectionApplied = true;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkExpGranted(DateTime nowUtc)
    {
        ExpGranted = true;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkSessionCompleted(DateTime nowUtc)
    {
        SessionCompleted = true;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkRevealedEventPublished(DateTime nowUtc)
    {
        RevealedEventPublished = true;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkFailed(string error, DateTime nowUtc, DateTime nextRepairAtUtc)
    {
        Status = ReadingRevealSagaStatus.Failed;
        LastError = string.IsNullOrWhiteSpace(error)
            ? "Unknown saga failure."
            : error.Trim();
        NextRepairAtUtc = nextRepairAtUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkCompensated(DateTime nowUtc)
    {
        RefundCompensated = true;
        Status = ReadingRevealSagaStatus.Compensated;
        NextRepairAtUtc = null;
        CompletedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public void MarkCompleted(DateTime nowUtc)
    {
        Status = ReadingRevealSagaStatus.Completed;
        NextRepairAtUtc = null;
        CompletedAtUtc = nowUtc;
        UpdatedAtUtc = nowUtc;
    }

    public bool IsTerminal()
    {
        return string.Equals(Status, ReadingRevealSagaStatus.Completed, StringComparison.Ordinal)
               || string.Equals(Status, ReadingRevealSagaStatus.Compensated, StringComparison.Ordinal);
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

    private static string? NormalizeOptional(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }
}
