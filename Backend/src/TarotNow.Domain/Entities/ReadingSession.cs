using TarotNow.Domain.Enums;

namespace TarotNow.Domain.Entities;

public class ReadingSession
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string SpreadType { get; private set; }
    

    // JSON array chứa index các lá bài đã rút. Ex: "[12, 45, 71]"
    public string? CardsDrawn { get; private set; } 
    
    // Status
    public bool IsCompleted { get; private set; }
    
    // Timestamp
    public DateTime CreatedAt { get; private set; }
    public DateTime? CompletedAt { get; private set; }

    // Dành cho EF Core
    protected ReadingSession() { }

    public ReadingSession(Guid userId, string spreadType)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        SpreadType = spreadType;
        IsCompleted = false;
        CreatedAt = DateTime.UtcNow;
    }

    public void CompleteSession(string cardsDrawnJson)
    {
        CardsDrawn = cardsDrawnJson;
        IsCompleted = true;
        CompletedAt = DateTime.UtcNow;
    }
}
