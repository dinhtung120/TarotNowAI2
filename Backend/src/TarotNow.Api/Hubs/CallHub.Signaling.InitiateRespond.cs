namespace TarotNow.Api.Hubs;

public partial class CallHub
{
    public async Task InitiateCall(string conversationId, string callType)
    {
        await RejectLegacySignalingAsync(nameof(InitiateCall), conversationId, callType);
    }

    public async Task RespondCall(string callSessionId, bool accept)
    {
        await RejectLegacySignalingAsync(nameof(RespondCall), callSessionId, accept);
    }

    private async Task RejectLegacySignalingAsync(string methodName, params object?[] args)
    {
        _logger.LogWarning("Legacy call signaling blocked: {Method} args={Args}", methodName, args);
        await SendClientErrorAsync("legacy_call_disabled", "Phiên bản gọi cũ đã bị vô hiệu hóa. Vui lòng cập nhật ứng dụng.");
    }
}
