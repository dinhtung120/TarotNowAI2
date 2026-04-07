

using TarotNow.Domain.Enums;
using System;

namespace TarotNow.Domain.Entities;

public class ReadingSession
{
    
    public string Id { get; private set; } = string.Empty;
    
    public string UserId { get; private set; } = string.Empty;
    
    
    public string SpreadType { get; private set; } = string.Empty;

        public string? Question { get; private set; }

    
    public string? CardsDrawn { get; private set; }

        public string? CurrencyUsed { get; private set; }

        public long AmountCharged { get; private set; }

    
    public bool IsCompleted { get; private set; }

    public DateTime CreatedAt { get; private set; }
    
    public DateTime? CompletedAt { get; private set; }

    
    public string? AiSummary { get; private set; }
    
    
    
    public IReadOnlyList<ReadingFollowup> Followups { get; private set; } = new List<ReadingFollowup>();

    protected ReadingSession() { }

        public ReadingSession(string userId, string spreadType, string? question = null, string? currencyUsed = null, long amountCharged = 0)
    {
        Id = Guid.NewGuid().ToString();
        UserId = userId;
        SpreadType = spreadType;
        Question = question;
        CurrencyUsed = currencyUsed;
        AmountCharged = amountCharged;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

        public static ReadingSession Rehydrate(ReadingSessionSnapshot snapshot)
    {
        return new ReadingSession(
            snapshot.UserId,
            snapshot.SpreadType,
            snapshot.Question,
            snapshot.CurrencyUsed,
            snapshot.AmountCharged)
        {
            Id = snapshot.Id,
            CardsDrawn = snapshot.CardsDrawn,
            IsCompleted = snapshot.IsCompleted,
            CreatedAt = snapshot.CreatedAt,
            CompletedAt = snapshot.CompletedAt,
            AiSummary = snapshot.AiSummary,
            Followups = snapshot.Followups ?? new List<ReadingFollowup>()
        };
    }

        public void CompleteSession(string cardsDrawnJson)
    {
        CardsDrawn = cardsDrawnJson;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}

public class ReadingFollowup
{
    public string Question { get; set; } = string.Empty;
    public string Answer { get; set; } = string.Empty;
}

public sealed class ReadingSessionSnapshot
{
    public string Id { get; init; } = string.Empty;
    public string UserId { get; init; } = string.Empty;
    public string SpreadType { get; init; } = string.Empty;
    public string? Question { get; init; }
    public string? CardsDrawn { get; init; }
    public string? CurrencyUsed { get; init; }
    public long AmountCharged { get; init; }
    public bool IsCompleted { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? CompletedAt { get; init; }
    public string? AiSummary { get; init; }
    public IReadOnlyList<ReadingFollowup>? Followups { get; init; }
}
