namespace TarotNow.Application.Common;

public sealed class CallJoinTicketDto
{
    public required CallSessionV2Dto Session { get; init; }

    public string LiveKitUrl { get; init; } = string.Empty;

    public string AccessToken { get; init; } = string.Empty;

    public string ParticipantIdentity { get; init; } = string.Empty;

    public CallTimeoutsDto Timeouts { get; init; } = new();
}

public sealed class CallTimeoutsDto
{
    public int RingTimeoutSeconds { get; init; }

    public int JoinTimeoutSeconds { get; init; }

    public int ReconnectGracePeriodSeconds { get; init; }
}
