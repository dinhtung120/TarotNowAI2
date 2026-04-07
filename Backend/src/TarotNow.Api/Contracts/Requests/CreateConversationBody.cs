namespace TarotNow.Api.Contracts.Requests;

public class CreateConversationBody
{
    public Guid ReaderId { get; set; }

        public int? SlaHours { get; set; }
}
