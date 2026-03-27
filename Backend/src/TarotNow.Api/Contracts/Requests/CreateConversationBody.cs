namespace TarotNow.Api.Contracts.Requests;

public class CreateConversationBody
{
    public Guid ReaderId { get; set; }

    /// <summary>
    /// SLA phản hồi mong muốn của reader sau khi accept: 6 | 12 | 24 (giờ).
    /// </summary>
    public int? SlaHours { get; set; }
}
