

namespace TarotNow.Domain.Entities;

public class ChatFinanceSession
{
    
    public Guid Id { get; set; }

        public string ConversationRef { get; set; } = string.Empty;

        public Guid UserId { get; set; }

        public Guid ReaderId { get; set; }

        public string Status { get; set; } = "pending";

        public long TotalFrozen { get; set; }

    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    
    public DateTime? UpdatedAt { get; set; }

    
    public virtual ICollection<ChatQuestionItem> QuestionItems { get; set; } = new List<ChatQuestionItem>();
}
