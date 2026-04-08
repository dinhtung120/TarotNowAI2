namespace TarotNow.Application.Common;

public sealed class CallSessionV2Dto
{
    public string Id { get; set; } = string.Empty;

    public string ConversationId { get; set; } = string.Empty;

    public string RoomName { get; set; } = string.Empty;

    public string InitiatorId { get; set; } = string.Empty;

    public string CalleeId { get; set; } = string.Empty;

    public string Type { get; set; } = CallTypeValues.Audio;

    public string Status { get; set; } = CallSessionV2Statuses.Requested;

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? AcceptedAt { get; set; }

    public DateTime? ConnectedAt { get; set; }

    public DateTime? EndedAt { get; set; }

    public string? EndReason { get; set; }

    public DateTime? InitiatorJoinedAt { get; set; }

    public DateTime? CalleeJoinedAt { get; set; }

    public bool IsLogCreated { get; set; }
}
