using TarotNow.Application.Common;

namespace TarotNow.Application.Interfaces;

public interface ICallV2Service
{
    Task<CallJoinTicketDto> StartAsync(
        Guid requesterId,
        string conversationId,
        string callType,
        CancellationToken ct = default);

    Task<CallJoinTicketDto> AcceptAsync(
        Guid requesterId,
        string callSessionId,
        CancellationToken ct = default);

    Task<CallJoinTicketDto> IssueTokenAsync(
        Guid requesterId,
        string callSessionId,
        CancellationToken ct = default);

    Task<CallSessionV2Dto> EndAsync(
        Guid requesterId,
        string callSessionId,
        string reason,
        CancellationToken ct = default);

    Task HandleWebhookAsync(string authorizationHeader, string payload, CancellationToken ct = default);

    CallTimeoutsDto GetTimeouts();
}
