using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TarotNow.Application.Common;
using TarotNow.Application.Interfaces;
using TarotNow.Infrastructure.Options;

namespace TarotNow.Infrastructure.Services.CallV2;

internal sealed partial class CallV2Service : ICallV2Service
{
    private readonly ICallSessionV2Repository _sessions;
    private readonly IConversationRepository _conversations;
    private readonly ILiveKitRoomGateway _rooms;
    private readonly LiveKitTokenFactory _tokenFactory;
    private readonly ICallRealtimePushService _realtimePush;
    private readonly IChatPushService _chatPush;
    private readonly IMediator _mediator;
    private readonly LiveKitOptions _liveKitOptions;
    private readonly CallV2Options _callOptions;
    private readonly ILogger<CallV2Service> _logger;

    public CallV2Service(
        ICallSessionV2Repository sessions,
        IConversationRepository conversations,
        ILiveKitRoomGateway rooms,
        LiveKitTokenFactory tokenFactory,
        ICallRealtimePushService realtimePush,
        IChatPushService chatPush,
        IMediator mediator,
        IOptions<LiveKitOptions> liveKitOptions,
        IOptions<CallV2Options> callOptions,
        ILogger<CallV2Service> logger)
    {
        _sessions = sessions;
        _conversations = conversations;
        _rooms = rooms;
        _tokenFactory = tokenFactory;
        _realtimePush = realtimePush;
        _chatPush = chatPush;
        _mediator = mediator;
        _liveKitOptions = liveKitOptions.Value;
        _callOptions = callOptions.Value;
        _logger = logger;
    }

    public CallTimeoutsDto GetTimeouts()
    {
        return new CallTimeoutsDto
        {
            RingTimeoutSeconds = Math.Max(5, _callOptions.RingTimeoutSeconds),
            JoinTimeoutSeconds = Math.Max(5, _callOptions.JoinTimeoutSeconds),
            ReconnectGracePeriodSeconds = Math.Max(5, _callOptions.ReconnectGracePeriodSeconds),
        };
    }
}
