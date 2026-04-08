namespace TarotNow.Application.Common;

public sealed class CallSessionV2Patch
{
    public string NewStatus { get; init; } = CallSessionV2Statuses.Requested;

    public IReadOnlyCollection<string>? ExpectedPreviousStatuses { get; init; }

    public DateTime? AcceptedAt { get; init; }

    public DateTime? ConnectedAt { get; init; }

    public DateTime? EndedAt { get; init; }

    public string? EndReason { get; init; }

    public DateTime? InitiatorJoinedAt { get; init; }

    public DateTime? CalleeJoinedAt { get; init; }

    public bool? IsLogCreated { get; init; }
}
